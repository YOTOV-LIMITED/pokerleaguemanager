using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using PokerLeagueManager.Commands.Domain.Infrastructure.Exceptions;
using PokerLeagueManager.Common.Commands.Infrastructure;
using PokerLeagueManager.Common.Events.Infrastructure;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Commands.Domain.Infrastructure
{
    public class EventRepository : IEventRepository
    {
        private IDatabaseLayer _databaseLayer;
        private IGuidService _guidService;
        private IDateTimeService _dateTimeService;
        private IEventServiceProxyFactory _eventServiceProxyFactory;

        public EventRepository(
            IDatabaseLayer databaseLayer,
            IGuidService guidService,
            IDateTimeService dateTimeService,
            IEventServiceProxyFactory eventServiceProxyFactory)
        {
            _databaseLayer = databaseLayer;
            _guidService = guidService;
            _dateTimeService = dateTimeService;
            _eventServiceProxyFactory = eventServiceProxyFactory;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:ParameterMustFollowComma", Justification = "For the DatabaseLayer calls this makes more sense.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public bool DoesAggregateExist(Guid aggregateId)
        {
            if (aggregateId == Guid.Empty)
            {
                return false;
            }

            int eventCount = (int)_databaseLayer.ExecuteScalar(
                "SELECT COUNT(*) FROM Events WHERE AggregateId = @AggregateId",
                "@AggregateId", aggregateId.ToString());

            if (eventCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:ParameterMustFollowComma", Justification = "For the DatabaseLayer calls this makes more sense.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        public T GetAggregateById<T>(Guid aggregateId) where T : IAggregateRoot
        {
            if (aggregateId == Guid.Empty)
            {
                throw new ArgumentException(string.Format("Invalid Aggregate ID ({0})", aggregateId.ToString()));
            }

            var constructor = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, System.Type.EmptyTypes, null);
            var result = (T)constructor.Invoke(null);

            var allEvents = _databaseLayer.GetDataTable(
                "SELECT EventData, EventType FROM Events WHERE AggregateId = @AggregateId ORDER BY EventTimestamp",
                "@AggregateId", aggregateId.ToString());

            if (allEvents.Rows.Count == 0)
            {
                throw new ArgumentException(string.Format("No Aggregate with the specified ID was found ({0})", aggregateId.ToString()));
            }

            foreach (DataRow row in allEvents.Rows)
            {
                var e = CreateEventFromDataRow(row);
                result.ApplyEvent(e);
                result.AggregateVersion = e.EventId;
            }

            return (T)result;
        }

        public void PublishEvents(IAggregateRoot aggRoot, ICommand c, Guid originalVersion)
        {
            if (aggRoot.PendingEvents == null || aggRoot.PendingEvents.Count == 0)
            {
                return;
            }

            if (aggRoot.AggregateVersion != originalVersion)
            {
                throw new OptimisticConcurrencyException(aggRoot, originalVersion);
            }

            PublishEvents(aggRoot, c);
        }

        public void PublishEvents(IAggregateRoot aggRoot, ICommand c)
        {
            if (aggRoot.PendingEvents == null || aggRoot.PendingEvents.Count == 0)
            {
                return;
            }

            ExecuteWithLockedAggregate(
                aggRoot.AggregateId,
                () =>
                {
                    ValidateAggregateOptimisticConcurrency(aggRoot);
                    PersistEventsInTransaction(aggRoot, c);
                });

            try
            {
                PublishEventsToSubscribers(aggRoot.PendingEvents);
            }
            catch (Exception ex)
            {
                throw new PublishEventFailedException(aggRoot, c, ex);
            }
        }

        public void PublishEvents(IEnumerable<IAggregateRoot> aggRoots, ICommand c)
        {
            foreach (var a in aggRoots)
            {
                PublishEvents(a, c);
            }
        }

        public void PublishAllUnpublishedEvents()
        {
            var allEvents = _databaseLayer.GetDataTable("SELECT EventData, EventType FROM Events WHERE Published = 0 ORDER BY EventTimestamp");

            var eventList = new List<IEvent>();

            foreach (DataRow row in allEvents.Rows)
            {
                eventList.Add(CreateEventFromDataRow(row));
            }

            PublishEventsToSubscribers(eventList);
        }

        private void ExecuteWithLockedAggregate(Guid aggregateId, Action work)
        {
            DeleteExpiredLocks();

            AcquireAggregateLock(aggregateId);
            work();
            ReleaseAggregateLock(aggregateId);
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:ParameterMustFollowComma", Justification = "For the DatabaseLayer calls this makes more sense.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void AcquireAggregateLock(Guid aggregateId)
        {
            try
            {
                _databaseLayer.ExecuteNonQuery(
                    "INSERT INTO AggregateLocks(AggregateId, LockExpiry) VALUES(@AggregateId, @LockExpiry)",
                    "@AggregateId", aggregateId.ToString(),
                    "@LockExpiry", _dateTimeService.UtcNow().AddMinutes(1));
            }
            catch (Exception ex)
            {
                throw new UnableToAcquireAggregateLockException(aggregateId, ex);
            }
        }

        private void DeleteExpiredLocks()
        {
            _databaseLayer.ExecuteNonQuery("DELETE FROM AggregateLocks WHERE LockExpiry < @CurrentDateTime", "@CurrentDateTime", _dateTimeService.UtcNow());
        }

        private void ValidateAggregateOptimisticConcurrency(IAggregateRoot aggRoot)
        {
            if (aggRoot.AggregateVersion == Guid.Empty)
            {
                return;
            }

            var currentVersion = GetAggregateVersion(aggRoot.AggregateId);

            if (currentVersion != aggRoot.AggregateVersion)
            {
                throw new OptimisticConcurrencyException(aggRoot, aggRoot.AggregateVersion, currentVersion);
            }
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:ParameterMustFollowComma", Justification = "For the DatabaseLayer calls this makes more sense.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private Guid GetAggregateVersion(Guid aggregateId)
        {
            return (Guid)_databaseLayer.ExecuteScalar(
                "SELECT TOP 1 EventId FROM Events WHERE AggregateId = @AggregateId ORDER BY EventTimestamp DESC",
                "@AggregateId", aggregateId.ToString());
        }

        private void PersistEventsInTransaction(IAggregateRoot aggRoot, ICommand c)
        {
            _databaseLayer.ExecuteInTransaction(() =>
            {
                foreach (var e in aggRoot.PendingEvents)
                {
                    PersistEventToDatabase(e, c, aggRoot.AggregateId);
                }
            });
        }

        private void ReleaseAggregateLock(Guid aggregateId)
        {
            _databaseLayer.ExecuteNonQuery("DELETE FROM AggregateLocks WHERE AggregateId = @AggregateId", "@AggregateId", aggregateId);
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:ParameterMustFollowComma", Justification = "For the DatabaseLayer calls this makes more sense.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void PersistEventToDatabase(IEvent e, ICommand c, Guid aggregateId)
        {
            e.EventId = _guidService.NewGuid();
            e.Timestamp = _dateTimeService.Now();
            e.CommandId = c.CommandId;
            e.AggregateId = aggregateId;

            var eventXml = SerializeEvent(e);
            var sql = "INSERT INTO Events(EventId, EventDateTime, CommandId, AggregateId, EventType, EventData, Published)";
            sql += " VALUES(@EventId, @EventDateTime, @CommandId, @AggregateId, @EventType, @EventData, @Published)";

            _databaseLayer.ExecuteNonQuery(
                sql,
                "@EventId", e.EventId,
                "@EventDateTime", e.Timestamp.ToUniversalTime().ToString("dd-MMM-yyyy HH:mm:ss.ff"),
                "@CommandId", e.CommandId,
                "@AggregateId", e.AggregateId,
                "@EventType", e.GetType().AssemblyQualifiedName,
                "@EventData", eventXml,
                "@Published", false);
        }

        private void PublishEventsToSubscribers(IEnumerable<IEvent> events)
        {
            var subscribers = GetSubscribers();

            foreach (var e in events)
            {
                foreach (var s in subscribers)
                {
                    s.HandleEvent(e);
                }

                MarkEventAsPublished(e);
            }
        }

        private void MarkEventAsPublished(IEvent e)
        {
            _databaseLayer.ExecuteNonQuery("UPDATE Events SET Published = 1 WHERE EventId = @EventId", "@EventId", e.EventId);
        }

        private IEnumerable<IEventService> GetSubscribers()
        {
            var subscriberTable = _databaseLayer.GetDataTable("SELECT * FROM Subscribers");

            foreach (DataRow row in subscriberTable.Rows)
            {
                yield return _eventServiceProxyFactory.Create(row);
            }
        }

        private string SerializeEvent<T>(T e)
        {
            var serializer = new DataContractSerializer(e.GetType());

            using (var memStream = new MemoryStream())
            {
                serializer.WriteObject(memStream, e);

                memStream.Position = 0;
                var reader = new StreamReader(memStream);

                return reader.ReadToEnd();
            }
        }

        private IEvent CreateEventFromDataRow(DataRow row)
        {
            var typeSections = ((string)row["EventType"]).Split(',');
            var typeDef = typeSections[0] + ", " + typeSections[1];

            var eventType = Type.GetType(typeDef, true);
            var ser = new DataContractSerializer(eventType);

            using (var xr = new XmlReaderFacade((string)row["EventData"]))
            {
                return (IEvent)ser.ReadObject(xr.XmlReader);
            }
        }
    }
}

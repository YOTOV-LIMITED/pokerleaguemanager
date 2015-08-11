using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PokerLeagueManager.Commands.Domain.Infrastructure;
using PokerLeagueManager.Common.Commands.Infrastructure;
using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Commands.Tests.Infrastructure
{
    public class FakeEventRepository : IEventRepository
    {
        public FakeEventRepository()
        {
            EventCommandList = new List<EventCommandPair>();
            EventList = new List<IEvent>();
            InitialEvents = new List<IEvent>();
        }

        public IList<EventCommandPair> EventCommandList { get; private set; }

        public IList<IEvent> EventList { get; private set; }

        public IEnumerable<IEvent> InitialEvents { get; set; }

        public void PublishEvents(IAggregateRoot aggRoot, ICommand c)
        {
            if (aggRoot == null)
            {
                throw new ArgumentNullException("aggRoot");
            }

            foreach (IEvent e in aggRoot.PendingEvents)
            {
                EventCommandList.Add(new EventCommandPair(e, c, aggRoot.AggregateId));
                EventList.Add(e);
            }
        }

        public void PublishEvents(IEnumerable<IAggregateRoot> aggRoots, ICommand c)
        {
            foreach (var a in aggRoots)
            {
                PublishEvents(a, c);
            }
        }

        public void PublishEvents(IAggregateRoot aggRoot, ICommand c, Guid originalVersion)
        {
            PublishEvents(aggRoot, c);
        }

        public T GetAggregateById<T>(Guid aggregateId) where T : IAggregateRoot
        {
            T aggRootInstance = default(T);

            if (InitialEvents != null)
            {
                var aggEvents = InitialEvents.Where<IEvent>(e => e.AggregateId == aggregateId);

                if (aggEvents.Count() > 0)
                {
                    var constructor = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, System.Type.EmptyTypes, null);
                    aggRootInstance = (T)constructor.Invoke(null);

                    aggRootInstance.AggregateId = aggregateId;

                    foreach (IEvent e in aggEvents)
                    {
                        aggRootInstance.ApplyEvent(e);
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("No Aggregate with the specified ID was found ({0})", aggregateId.ToString()));
                }
            }
            else
            {
                throw new ArgumentException(string.Format("No Aggregate with the specified ID was found ({0})", aggregateId.ToString()));
            }

            return aggRootInstance;
        }

        public bool DoesAggregateExist(Guid aggregateId)
        {
            if (aggregateId == Guid.Empty || InitialEvents == null)
            {
                return false;
            }

            return InitialEvents.Any(e => e.AggregateId == aggregateId);
        }

        public void PublishAllUnpublishedEvents()
        {
            throw new NotImplementedException();
        }
    }
}

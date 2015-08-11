using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PokerLeagueManager.Common.Events.Infrastructure;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Queries.Core.Infrastructure
{
    public class EventHandlerFactory : IEventHandlerFactory
    {
        private IQueryDataStore _queryDataStore;
        private IIdempotencyChecker _idempotencyChecker;
        private IDatabaseLayer _databaseLayer;

        public EventHandlerFactory(IQueryDataStore queryDataStore, IIdempotencyChecker idempotencyChecker, IDatabaseLayer databaseLayer)
        {
            _queryDataStore = queryDataStore;
            _idempotencyChecker = idempotencyChecker;
            _databaseLayer = databaseLayer;

            _idempotencyChecker.DatabaseLayer = _databaseLayer;
        }

        public void HandleEvent<T>(T e) where T : IEvent
        {
            if (e == null)
            {
                throw new ArgumentNullException("e", "Cannot handle a null event.");
            }

            if (_idempotencyChecker.CheckIdempotency(e.EventId))
            {
                return;
            }

            _databaseLayer.ExecuteInTransaction(() =>
                {
                    foreach (var handler in FindEventHandlers<T>())
                    {
                        handler.Handle(e);
                    }

                    _idempotencyChecker.MarkEventAsProcessed(e.EventId);
                });
        }

        public void HandleEvent(IEvent e)
        {
            var executeEventHandler = from m in typeof(EventHandlerFactory).GetMethods()
                                      where m.Name == "HandleEvent" && m.ContainsGenericParameters && m.IsGenericMethod && m.IsGenericMethodDefinition
                                      select m;

            MethodInfo generic = executeEventHandler.First().MakeGenericMethod(e.GetType());
            generic.Invoke(this, new object[] { e });
        }

        private IEnumerable<IHandlesEvent<T>> FindEventHandlers<T>() where T : IEvent
        {
            var matchingTypes = typeof(IHandlesEvent<>).FindHandlers<T>(Assembly.GetExecutingAssembly());

            foreach (var handler in matchingTypes)
            {
                var result = (IHandlesEvent<T>)UnitySingleton.Container.Resolve(handler, null);
                result.QueryDataStore = _queryDataStore;

                yield return result;
            }
        }
    }
}

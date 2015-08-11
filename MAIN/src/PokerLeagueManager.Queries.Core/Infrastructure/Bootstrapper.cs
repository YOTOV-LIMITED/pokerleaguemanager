using Microsoft.Practices.Unity;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Queries.Core.Infrastructure
{
    public static class Bootstrapper
    {
        private static bool _hasBootstrapped = false;

        public static void Bootstrap()
        {
            if (!_hasBootstrapped)
            {
                UnitySingleton.Container.RegisterType<IEventHandlerFactory, EventHandlerFactory>();
                UnitySingleton.Container.RegisterType<IQueryDataStore, QueryDataStore>();
                UnitySingleton.Container.RegisterType<IQueryService, QueryHandler>();
                UnitySingleton.Container.RegisterType<IIdempotencyChecker, IdempotencyChecker>();

                PokerLeagueManager.Common.Utilities.Bootstrapper.Bootstrap();

                _hasBootstrapped = true;
            }
        }
    }
}
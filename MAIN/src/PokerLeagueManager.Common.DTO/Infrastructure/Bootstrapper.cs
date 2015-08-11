using Microsoft.Practices.Unity;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Common.DTO.Infrastructure
{
    public static class Bootstrapper
    {
        private static bool _hasBootstrapped = false;

        public static void Bootstrap()
        {
            if (!_hasBootstrapped)
            {
                UnitySingleton.Container.RegisterType<IDtoFactory, DtoFactory>();
                UnitySingleton.Container.RegisterType<IQueryService, QueryServiceProxy>();

                PokerLeagueManager.Common.Utilities.Bootstrapper.Bootstrap();

                _hasBootstrapped = true;
            }
        }
    }
}
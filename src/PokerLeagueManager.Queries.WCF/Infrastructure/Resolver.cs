using Microsoft.Practices.Unity;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Queries.WCF.Infrastructure
{
    public static class Resolver
    {
        private static bool _hasBootstrapped;

        public static IUnityContainer Container
        {
            get
            {
                if (!_hasBootstrapped)
                {
                    Bootstrap();
                }

                return UnitySingleton.Container;
            }
        }

        public static void Bootstrap()
        {
            PokerLeagueManager.Common.Utilities.Bootstrapper.Bootstrap();
            PokerLeagueManager.Common.DTO.Infrastructure.Bootstrapper.Bootstrap();
            PokerLeagueManager.Common.Events.Infrastructure.Bootstrapper.Bootstrap();
            PokerLeagueManager.Queries.Core.Infrastructure.Bootstrapper.Bootstrap();

            _hasBootstrapped = true;
        }
    }
}
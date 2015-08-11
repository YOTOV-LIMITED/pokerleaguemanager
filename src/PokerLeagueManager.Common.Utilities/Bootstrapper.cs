using System.Security.Principal;
using Microsoft.Practices.Unity;

namespace PokerLeagueManager.Common.Utilities
{
    public static class Bootstrapper
    {
        private static bool _hasBootstrapped = false;

        public static void Bootstrap()
        {
            if (!_hasBootstrapped)
            {
                UnitySingleton.Container.RegisterType<IDateTimeService, DateTimeService>();
                UnitySingleton.Container.RegisterType<IGuidService, GuidService>();
                UnitySingleton.Container.RegisterType<IDatabaseLayer, SqlServerDatabaseLayer>();
                UnitySingleton.Container.RegisterInstance<IIdentity>(System.Security.Principal.WindowsPrincipal.Current.Identity);

                _hasBootstrapped = true;
            }
        }
    }
}
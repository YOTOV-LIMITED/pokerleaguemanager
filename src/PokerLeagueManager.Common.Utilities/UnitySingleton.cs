using Microsoft.Practices.Unity;

namespace PokerLeagueManager.Common.Utilities
{
    public static class UnitySingleton
    {
        private static IUnityContainer _container;

        public static IUnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new UnityContainer();
                }

                return _container;
            }
        }
    }
}

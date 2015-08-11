using log4net;
using Microsoft.Practices.Unity;
using PokerLeagueManager.Common.Utilities;
using PokerLeagueManager.UI.Wpf.ViewModels;
using PokerLeagueManager.UI.Wpf.Views;

namespace PokerLeagueManager.UI.Wpf
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
            UnitySingleton.Container.RegisterType<IEnterGameResultsViewModel, EnterGameResultsViewModel>();
            UnitySingleton.Container.RegisterType<IViewGamesListViewModel, ViewGamesListViewModel>();
            UnitySingleton.Container.RegisterType<IViewGameResultsViewModel, ViewGameResultsViewModel>();
            UnitySingleton.Container.RegisterType<IPlayerStatisticsViewModel, PlayerStatisticsViewModel>();
            UnitySingleton.Container.RegisterType<IPlayerGamesViewModel, PlayerGamesViewModel>();

            UnitySingleton.Container.RegisterType<IViewGamesListView, ViewGamesListView>();
            UnitySingleton.Container.RegisterType<IEnterGameResultsView, EnterGameResultsView>();
            UnitySingleton.Container.RegisterType<IViewGameResultsView, ViewGameResultsView>();
            UnitySingleton.Container.RegisterType<IPlayerStatisticsView, PlayerStatisticsView>();
            UnitySingleton.Container.RegisterType<IPlayerGamesView, PlayerGamesView>();

            UnitySingleton.Container.RegisterType<ILog>(new InjectionFactory(x => LogManager.GetLogger(string.Empty)));

            PokerLeagueManager.Common.DTO.Infrastructure.Bootstrapper.Bootstrap();
            PokerLeagueManager.Common.Commands.Infrastructure.Bootstrapper.Bootstrap();
            PokerLeagueManager.Common.Utilities.Bootstrapper.Bootstrap();

            _hasBootstrapped = true;
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using log4net;
using Microsoft.Practices.Unity;
using PokerLeagueManager.Common.Commands;
using PokerLeagueManager.Common.Commands.Infrastructure;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.UI.Wpf.Infrastructure;
using PokerLeagueManager.UI.Wpf.Views;

namespace PokerLeagueManager.UI.Wpf.ViewModels
{
    public class ViewGamesListViewModel : BaseViewModel, INotifyPropertyChanged, IViewGamesListViewModel
    {
        private ObservableCollection<GetGamesListDto> _games;

        public ViewGamesListViewModel(ICommandService commandService, IQueryService queryService, IMainWindow mainWindow, ILog logger)
            : base(commandService, queryService, mainWindow, logger)
        {
            _games = new ObservableCollection<GetGamesListDto>(_QueryService.GetGamesList());

            PlayersCommand = new RelayCommand(x => NavigateToPlayersView());
            AddGameCommand = new RelayCommand(x => AddGame());
            DeleteGameCommand = new RelayCommand(x => DeleteGame(), x => CanDeleteGame());
            GameDoubleClickCommand = new RelayCommand(x => GameDoubleClick());

            Height = 400;
            Width = 385;
            WindowTitle = "View Games";
        }

        private bool CanDeleteGame()
        {
            return _games.Count > 0 && SelectedGameIndex >= 0;
        }

        public IEnumerable<string> Games
        {
            get
            {
                return _games.OrderByDescending(g => g.GameDate)
                             .Select(g => string.Format("{0} - {1} [${2}]", g.GameDate.ToString("dd-MMM-yyyy"), g.Winner, g.Winnings));
            }
        }

        public int SelectedGameIndex { get; set; }

        public System.Windows.Input.ICommand PlayersCommand { get; set; }

        public System.Windows.Input.ICommand AddGameCommand { get; set; }

        public System.Windows.Input.ICommand DeleteGameCommand { get; set; }

        public System.Windows.Input.ICommand GameDoubleClickCommand { get; set; }

        private void NavigateToPlayersView()
        {
            var view = Resolver.Container.Resolve<IPlayerStatisticsView>();
            _MainWindow.ShowView(view);
        }

        private void AddGame()
        {
            var view = Resolver.Container.Resolve<IEnterGameResultsView>();
            _MainWindow.ShowView(view);
        }

        private void DeleteGame()
        {
            var gameId = GetSelectedGame().GameId;
            _CommandService.ExecuteCommand(new DeleteGameCommand() { GameId = gameId });

            _games = new ObservableCollection<GetGamesListDto>(_QueryService.GetGamesList());
            OnPropertyChanged("Games");
        }

        private void GameDoubleClick()
        {
            if (_games.Count() == 0 || SelectedGameIndex < 0)
            {
                return;
            }

            var view = Resolver.Container.Resolve<IViewGameResultsView>();
            view.GameId = GetSelectedGame().GameId;
            _MainWindow.ShowView(view);
        }

        private GetGamesListDto GetSelectedGame()
        {
            return _games.OrderByDescending(g => g.GameDate).ElementAt(SelectedGameIndex);
        }
    }
}

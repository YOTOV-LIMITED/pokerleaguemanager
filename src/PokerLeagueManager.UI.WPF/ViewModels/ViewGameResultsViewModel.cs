using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using log4net;
using Microsoft.Practices.Unity;
using PokerLeagueManager.Common.Commands.Infrastructure;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.UI.Wpf.Infrastructure;
using PokerLeagueManager.UI.Wpf.Views;

namespace PokerLeagueManager.UI.Wpf.ViewModels
{
    public class ViewGameResultsViewModel : BaseViewModel, INotifyPropertyChanged, IViewGameResultsViewModel
    {
        public IEnumerable<string> Players { get; private set; }

        public ViewGameResultsViewModel(ICommandService commandService, IQueryService queryService, IMainWindow mainWindow, ILog logger)
            : base(commandService, queryService, mainWindow, logger)
        {
            CloseCommand = new RelayCommand(x => this.Close());

            Height = 400;
            WindowTitle = "View Game Results";
        }

        public string GameDate { get; set; }

        private Guid _gameId;

        public Guid GameId
        {
            get
            {
                return _gameId;
            }

            set
            {
                _gameId = value;

                var gameResults = _QueryService.GetGameResults(_gameId);

                GameDate = gameResults.GameDate.ToString("d-MMM-yyyy");
                OnPropertyChanged("GameDate");

                Players = gameResults.Players.OrderBy(p => p.Placing)
                                             .Select(p => string.Format("{0} - {1} [Win: ${2}] [Pay: ${3}]", p.Placing, p.PlayerName, p.Winnings, p.PayIn));
                OnPropertyChanged("Players");
            }
        }

        public System.Windows.Input.ICommand CloseCommand { get; set; }

        private void Close()
        {
            _MainWindow.ShowView(Resolver.Container.Resolve<IViewGamesListView>());
        }
    }
}

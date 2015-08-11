using System;
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
    public class EnterGameResultsViewModel : BaseViewModel, INotifyPropertyChanged, IEnterGameResultsViewModel
    {
        private ObservableCollection<EnterGameResultsCommand.GamePlayer> _playerCommands;

        public EnterGameResultsViewModel(ICommandService commandService, IQueryService queryService, IMainWindow mainWindow, ILog logger)
            : base(commandService, queryService, mainWindow, logger)
        {
            ResetPlayerCommands();

            AddPlayerCommand = new RelayCommand(x => this.AddPlayer(), x => this.CanAddPlayer());
            DeletePlayerCommand = new RelayCommand(x => this.DeletePlayer(), x => this.CanDeletePlayer());
            SaveGameCommand = new RelayCommand(x => this.SaveGame(), x => this.CanSaveGame());
            CancelCommand = new RelayCommand(x => this.Cancel());

            ClearNewPlayer();

            Height = 400;
            WindowTitle = "Enter Game Results";
        }

        public DateTime? GameDate { get; set; }

        public string NewPlayerName { get; set; }

        public string NewPlacing { get; set; }

        public string NewWinnings { get; set; }

        public string NewPayIn { get; set; }

        public IEnumerable<string> Players
        {
            get
            {
                return _playerCommands.OrderBy(p => p.Placing)
                                      .Select(p => string.Format("{0} - {1} [Win: ${2}] [Pay: ${3}]", p.Placing, p.PlayerName, p.Winnings, p.PayIn));
            }
        }

        public System.Windows.Input.ICommand AddPlayerCommand { get; set; }

        public System.Windows.Input.ICommand DeletePlayerCommand { get; set; }

        public System.Windows.Input.ICommand SaveGameCommand { get; set; }

        public System.Windows.Input.ICommand CancelCommand { get; set; }

        public int SelectedPlayerIndex { get; set; }

        private void ResetPlayerCommands()
        {
            _playerCommands = new ObservableCollection<EnterGameResultsCommand.GamePlayer>();
            _playerCommands.CollectionChanged += (x, y) => OnPropertyChanged("Players");
        }

        private bool CanSaveGame()
        {
            return GameDate != null;
        }

        private void SaveGame()
        {
            if (!CanSaveGame())
            {
                throw new InvalidOperationException("SaveGame should never be called if CanSaveGame returns false");
            }

            var gameCommand = new EnterGameResultsCommand();

            gameCommand.GameDate = this.GameDate.GetValueOrDefault();
            gameCommand.Players = _playerCommands;

            var commandResult = ExecuteCommand(gameCommand);

            if (commandResult)
            {
                Cancel();
            }
        }

        private void Cancel()
        {
            _MainWindow.ShowView(Resolver.Container.Resolve<IViewGamesListView>());
        }

        private void ClearNewPlayer()
        {
            this.NewPlayerName = string.Empty;
            this.NewPlacing = string.Empty;
            this.NewWinnings = "0";
            this.NewPayIn = "0";

            OnPropertyChanged("NewPlayerName");
            OnPropertyChanged("NewPlacing");
            OnPropertyChanged("NewWinnings");
            OnPropertyChanged("NewPayIn");
        }

        private bool CanAddPlayer()
        {
            if (string.IsNullOrWhiteSpace(NewPlayerName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewPlacing))
            {
                return false;
            }

            int temp;

            if (!int.TryParse(NewPlacing, out temp))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(NewWinnings) && !int.TryParse(NewWinnings, out temp))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(NewPayIn) && !int.TryParse(NewPayIn, out temp))
            {
                return false;
            }

            return true;
        }

        private bool CanDeletePlayer()
        {
            return _playerCommands.Count > 0 && SelectedPlayerIndex >= 0;
        }

        private void DeletePlayer()
        {
            if (!CanDeletePlayer())
            {
                throw new InvalidOperationException("DeletePlayer should never be called if CanDeletePlayer returns false");
            }

            _playerCommands.Remove(_playerCommands.OrderBy(p => p.Placing).ElementAt(SelectedPlayerIndex));
        }

        private void AddPlayer()
        {
            if (!CanAddPlayer())
            {
                throw new InvalidOperationException("AddPlayer should never be called if CanAddPlayer returns false");
            }

            var newPlayer = new EnterGameResultsCommand.GamePlayer();

            newPlayer.PlayerName = NewPlayerName;
            newPlayer.Placing = int.Parse(NewPlacing);

            if (string.IsNullOrWhiteSpace(NewWinnings))
            {
                newPlayer.Winnings = 0;
            }
            else
            {
                newPlayer.Winnings = int.Parse(NewWinnings);
            }

            if (string.IsNullOrWhiteSpace(NewPayIn))
            {
                newPlayer.PayIn = 0;
            }
            else
            {
                newPlayer.PayIn = int.Parse(NewPayIn);
            }

            _playerCommands.Add(newPlayer);

            ClearNewPlayer();
        }
    }
}

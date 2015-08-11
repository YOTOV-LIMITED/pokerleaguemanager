using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PokerLeagueManager.UI.Wpf.ViewModels
{
    public interface IEnterGameResultsViewModel : INotifyPropertyChanged
    {
        System.Windows.Input.ICommand AddPlayerCommand { get; set; }

        System.Windows.Input.ICommand SaveGameCommand { get; set; }

        System.Windows.Input.ICommand CancelCommand { get; set; }

        DateTime? GameDate { get; set; }

        string NewPlacing { get; set; }

        string NewPlayerName { get; set; }

        string NewWinnings { get; set; }

        IEnumerable<string> Players { get; }
    }
}

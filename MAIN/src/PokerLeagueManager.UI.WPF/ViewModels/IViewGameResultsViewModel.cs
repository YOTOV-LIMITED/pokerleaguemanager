using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PokerLeagueManager.UI.Wpf.ViewModels
{
    public interface IViewGameResultsViewModel : INotifyPropertyChanged
    {
        System.Windows.Input.ICommand CloseCommand { get; set; }

        string GameDate { get; set; }

        IEnumerable<string> Players { get; }

        Guid GameId { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel;

namespace PokerLeagueManager.UI.Wpf.ViewModels
{
    public interface IPlayerStatisticsViewModel : INotifyPropertyChanged
    {
        System.Windows.Input.ICommand GamesCommand { get; set; }

        System.Windows.Input.ICommand PlayerDoubleClickCommand { get; set; }

        IEnumerable<string> Players { get; }

        int SelectedPlayerIndex { get; set; }
    }
}

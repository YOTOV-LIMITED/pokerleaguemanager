using System.Windows.Controls;
using PokerLeagueManager.UI.Wpf.ViewModels;

namespace PokerLeagueManager.UI.Wpf.Views
{
    public partial class ViewGamesListView : UserControl, IViewGamesListView
    {
        public ViewGamesListView(IViewGamesListViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}

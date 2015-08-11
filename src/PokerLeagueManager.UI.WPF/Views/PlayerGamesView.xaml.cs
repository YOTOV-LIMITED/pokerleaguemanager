using System.Windows.Controls;
using PokerLeagueManager.UI.Wpf.ViewModels;

namespace PokerLeagueManager.UI.Wpf.Views
{
    public partial class PlayerGamesView : UserControl, IPlayerGamesView
    {
        private IPlayerGamesViewModel _viewModel;

        public PlayerGamesView(IPlayerGamesViewModel viewModel)
        {
            this.DataContext = viewModel;
            _viewModel = viewModel;

            InitializeComponent();
        }

        public string PlayerName
        {
            get { return _viewModel.PlayerName; }

            set { _viewModel.PlayerName = value; }
        }
    }
}

using System;
using System.Windows.Controls;
using PokerLeagueManager.UI.Wpf.ViewModels;

namespace PokerLeagueManager.UI.Wpf.Views
{
    public partial class ViewGameResultsView : UserControl, IViewGameResultsView
    {
        private IViewGameResultsViewModel _viewModel;

        public ViewGameResultsView(IViewGameResultsViewModel viewModel)
        {
            this.DataContext = viewModel;
            _viewModel = viewModel;

            InitializeComponent();
        }

        public Guid GameId
        {
            get { return _viewModel.GameId; }

            set { _viewModel.GameId = value; }
        }
    }
}

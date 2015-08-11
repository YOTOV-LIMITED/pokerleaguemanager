using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using log4net;
using Microsoft.Practices.Unity;
using PokerLeagueManager.Common.Commands.Infrastructure;
using PokerLeagueManager.UI.Wpf.Infrastructure;
using PokerLeagueManager.UI.Wpf.Views;

namespace PokerLeagueManager.UI.Wpf
{
    public partial class MainWindow : Window, IMainWindow
    {
        private readonly ILog _logger;

        public MainWindow()
        {
            InitializeComponent();

            Application.Current.DispatcherUnhandledException += GlobalExceptionHandler;
            Resolver.Container.RegisterInstance<IMainWindow>(this);
            _logger = Resolver.Container.Resolve<ILog>();

            ShowView(Resolver.Container.Resolve<ViewGamesListView>());
        }

        public void ShowView(object view)
        {
            this.Content = view;

            var viewControl = (UserControl)view;
            this.MinHeight = viewControl.MinHeight + 30;
            this.MinWidth = viewControl.MinWidth + 30;
            this.MaxHeight = viewControl.MaxHeight + 30;
            this.MaxWidth = viewControl.MaxWidth + 30;

            var viewModel = (BaseViewModel)viewControl.DataContext;
            this.Height = viewModel.Height + 30;
            this.Width = viewModel.Width + 30;
            this.Title = viewModel.WindowTitle;
        }

        public void ShowWarning(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void SetWidth(int width)
        {
            this.Width = width;
        }

        public void SetHeight(int height)
        {
            this.Height = height;
        }

        public void SetMinWidth(int minWidth)
        {
            this.MinWidth = minWidth;
        }

        public void SetMinHeight(int minHeight)
        {
            this.MinHeight = minHeight;
        }

        public void SetMaxWidth(int maxWidth)
        {
            this.MaxWidth = maxWidth;
        }

        public void SetMaxHeight(int maxHeight)
        {
            this.MaxHeight = maxHeight;
        }

        public void SetWindowTitle(string title)
        {
            this.Title = title;
        }

        private void GlobalExceptionHandler(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.Fatal("Unhandled Exception", e.Exception);
            MessageBox.Show("An unexpected error has occurred. The details have been logged. The application will now shutdown.", "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            Application.Current.Shutdown();
        }
    }
}

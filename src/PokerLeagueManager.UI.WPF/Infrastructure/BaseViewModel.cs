using System;
using System.ComponentModel;
using System.ServiceModel;
using log4net;
using PokerLeagueManager.Common.Commands.Infrastructure;
using PokerLeagueManager.Common.DTO;

namespace PokerLeagueManager.UI.Wpf.Infrastructure
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel(ICommandService commandService, IQueryService queryService, IMainWindow mainWindow, ILog logger)
        {
            _MainWindow = mainWindow;
            _CommandService = commandService;
            _QueryService = queryService;
            _Logger = logger;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Width { get; set; }

        public int Height { get; set; }

        public string WindowTitle { get; set; }

        protected IMainWindow _MainWindow { get; private set; }

        protected ICommandService _CommandService { get; private set; }

        protected IQueryService _QueryService { get; private set; }

        protected ILog _Logger { get; private set; }

        protected bool ExecuteCommand(ICommand command)
        {
            try
            {
                _Logger.Info(string.Format("Executing Command {0} [{1}]", command.GetType().Name, command.CommandId.ToString()));
                _CommandService.ExecuteCommand(command);
                return true;
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }

        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }
        }

        private bool HandleException(Exception ex)
        {
            var actionSucceeded = false;
            
            var fault = ex as FaultException<ExceptionDetail>;

            if (fault != null)
            {
                if (fault.Detail.Type.StartsWith("PokerLeagueManager"))
                {
                    _Logger.Warn(fault.Detail.Type);
                    _Logger.Warn(fault.Detail.Message);

                    if (fault.Detail.Type.Contains("PublishEventFailedException"))
                    {
                        _MainWindow.ShowWarning("Action Succeeded", fault.Detail.Message);
                        actionSucceeded = true;
                    }
                    else
                    {
                        _MainWindow.ShowWarning("Action Failed", fault.Detail.Message);
                    }
                }
                else
                {
                    _Logger.Error(fault.Detail.Type);
                    _Logger.Error(fault.Detail.Message);
                    _MainWindow.ShowError("Action Failed", fault.Detail.Message);
                }
            }
            else
            {
                _Logger.Error("Command Failed", ex);
                _MainWindow.ShowError("Action Failed", ex.Message);
            }

            return actionSucceeded;
        }
    }
}

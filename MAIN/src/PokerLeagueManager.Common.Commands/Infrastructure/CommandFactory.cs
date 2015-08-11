using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Common.Commands.Infrastructure
{
    public class CommandFactory : ICommandFactory
    {
        private OperationContext _currentContext;
        private IGuidService _guidService;
        private IDateTimeService _dateTimeService;

        public CommandFactory(OperationContext currentContext, IGuidService guidService, IDateTimeService dateTimeService)
        {
            _currentContext = currentContext;
            _guidService = guidService;
            _dateTimeService = dateTimeService;
        }

        public T Create<T>() where T : ICommand, new()
        {
            var result = new T();

            return Create(result);
        }

        public T Create<T>(T cmd) where T : ICommand
        {
            if (_currentContext != null && 
                _currentContext.ClaimsPrincipal != null && 
                _currentContext.ClaimsPrincipal.Identity != null && 
                !string.IsNullOrWhiteSpace(_currentContext.ClaimsPrincipal.Identity.Name))
            {
                cmd.User = _currentContext.ClaimsPrincipal.Identity.Name;
            }
            else
            {
                cmd.User = "Unknown";
            }

            MessageProperties prop = _currentContext.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            cmd.IPAddress = endpoint.Address;

            if (cmd.CommandId == Guid.Empty)
            {
                cmd.CommandId = _guidService.NewGuid();
            }

            cmd.Timestamp = _dateTimeService.Now();

            return cmd;
        }
    }
}

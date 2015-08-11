using PokerLeagueManager.Common.Commands.Infrastructure;

namespace PokerLeagueManager.Commands.Domain.Infrastructure
{
    public interface ICommandHandlerFactory
    {
        void ExecuteCommand<T>(T command) where T : ICommand;

        void ExecuteCommand(ICommand command);
    }
}

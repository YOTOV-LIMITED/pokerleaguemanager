using PokerLeagueManager.Commands.Domain.Aggregates;
using PokerLeagueManager.Commands.Domain.Infrastructure;
using PokerLeagueManager.Common.Commands;

namespace PokerLeagueManager.Commands.Domain.CommandHandlers
{
    public class DeleteGameCommandHandler : BaseCommandHandler, IHandlesCommand<DeleteGameCommand>
    {
        public void Execute(DeleteGameCommand command)
        {
            var game = Repository.GetAggregateById<Game>(command.GameId);

            game.DeleteGame();

            this.Repository.PublishEvents(game, command);
        }
    }
}

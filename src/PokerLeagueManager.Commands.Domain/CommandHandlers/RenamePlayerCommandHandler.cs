using System.Linq;
using PokerLeagueManager.Commands.Domain.Aggregates;
using PokerLeagueManager.Commands.Domain.Infrastructure;
using PokerLeagueManager.Common.Commands;

namespace PokerLeagueManager.Commands.Domain.CommandHandlers
{
    public class RenamePlayerCommandHandler : BaseCommandHandler, IHandlesCommand<RenamePlayerCommand>
    {
        public void Execute(RenamePlayerCommand command)
        {
            var games = QueryService.GetGamesWithPlayer(command.OldPlayerName);
            var aggregates = games.Select(g => Repository.GetAggregateById<Game>(g.GameId)).ToList();

            foreach (var g in aggregates)
            {
                g.RenamePlayer(command.OldPlayerName, command.NewPlayerName);
            }

            this.Repository.PublishEvents(aggregates, command);
        }
    }
}

using System.Linq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core.EventHandlers
{
    public class GetGamesListHandler : BaseHandler, IHandlesEvent<GameCreatedEvent>, IHandlesEvent<PlayerAddedToGameEvent>, IHandlesEvent<GameDeletedEvent>, IHandlesEvent<PlayerRenamedEvent>
    {
        public void Handle(GameCreatedEvent e)
        {
            QueryDataStore.Insert(new GetGamesListDto()
            {
                GameId = e.AggregateId,
                GameDate = e.GameDate,
                Winnings = 0,
                Winner = string.Empty
            });
        }

        public void Handle(PlayerAddedToGameEvent e)
        {
            var game = QueryDataStore.GetData<GetGamesListDto>().First(x => x.GameId == e.AggregateId);

            if (e.Placing == 1)
            {
                game.Winner = e.PlayerName;
                game.Winnings = e.Winnings;
            }

            QueryDataStore.SaveChanges();
        }

        public void Handle(GameDeletedEvent e)
        {
            var dto = QueryDataStore.GetData<GetGamesListDto>().Single(x => x.GameId == e.AggregateId);

            QueryDataStore.Delete<GetGamesListDto>(dto);
        }

        public void Handle(PlayerRenamedEvent e)
        {
            var games = QueryDataStore.GetData<GetGamesListDto>().Where(x => x.Winner == e.OldPlayerName);

            foreach (var g in games)
            {
                g.Winner = e.NewPlayerName;
            }

            QueryDataStore.SaveChanges();
        }
    }
}

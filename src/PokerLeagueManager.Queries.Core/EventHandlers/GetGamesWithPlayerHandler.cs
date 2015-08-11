using System.Linq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core.EventHandlers
{
    public class GetGamesWithPlayerHandler : BaseHandler, IHandlesEvent<PlayerAddedToGameEvent>, IHandlesEvent<GameDeletedEvent>, IHandlesEvent<PlayerRenamedEvent>
    {
        public void Handle(PlayerAddedToGameEvent e)
        {
            var dto = new GetGamesWithPlayerDto();
            dto.GameId = e.AggregateId;
            dto.PlayerName = e.PlayerName;

            QueryDataStore.Insert<GetGamesWithPlayerDto>(dto);
        }

        public void Handle(GameDeletedEvent e)
        {
            var dtos = QueryDataStore.GetData<GetGamesWithPlayerDto>().Where(x => x.GameId == e.AggregateId).ToList();

            foreach (var d in dtos)
            {
                QueryDataStore.Delete<GetGamesWithPlayerDto>(d);
            }
        }

        public void Handle(PlayerRenamedEvent e)
        {
            var players = QueryDataStore.GetData<GetGamesWithPlayerDto>().Where(x => x.PlayerName == e.OldPlayerName).ToList();

            foreach (var p in players)
            {
                p.PlayerName = e.NewPlayerName;
            }

            QueryDataStore.SaveChanges();
        }
    }
}

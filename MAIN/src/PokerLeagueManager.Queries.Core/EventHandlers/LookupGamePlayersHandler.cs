using System.Linq;
using PokerLeagueManager.Common.DTO.DataTransferObjects.Lookups;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core.EventHandlers
{
    public class LookupGamePlayersHandler : BaseHandler, IHandlesEvent<PlayerAddedToGameEvent>, IHandlesEvent<PlayerRenamedEvent>
    {
        public void Handle(PlayerAddedToGameEvent e)
        {
            var dto = new LookupGamePlayersDto();

            dto.GameId = e.AggregateId;
            dto.Winnings = e.Winnings;
            dto.PayIn = e.PayIn;
            dto.PlayerName = e.PlayerName;

            QueryDataStore.Insert<LookupGamePlayersDto>(dto);
        }

        public void Handle(PlayerRenamedEvent e)
        {
            var players = QueryDataStore.GetData<LookupGamePlayersDto>().Where(x => x.PlayerName == e.OldPlayerName).ToList();

            foreach (var p in players)
            {
                p.PlayerName = e.NewPlayerName;
            }

            QueryDataStore.SaveChanges();
        }
    }
}

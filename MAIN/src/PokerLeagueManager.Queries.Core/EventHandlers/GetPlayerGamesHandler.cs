using System.Linq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.DTO.DataTransferObjects.Lookups;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core.EventHandlers
{
    public class GetPlayerGamesHandler : BaseHandler, IHandlesEvent<PlayerAddedToGameEvent>, IHandlesEvent<GameDeletedEvent>, IHandlesEvent<PlayerRenamedEvent>
    {
        public void Handle(PlayerAddedToGameEvent e)
        {
            var gameDateDto = QueryDataStore.GetData<LookupGameDatesDto>().Single(x => x.GameId == e.AggregateId);

            var dto = new GetPlayerGamesDto();
            dto.GameId = e.AggregateId;
            dto.GameDate = gameDateDto.GameDate;
            dto.PlayerName = e.PlayerName;
            dto.Placing = e.Placing;
            dto.Winnings = e.Winnings;
            dto.PayIn = e.PayIn;

            QueryDataStore.Insert<GetPlayerGamesDto>(dto);
        }

        public void Handle(GameDeletedEvent e)
        {
            var games = QueryDataStore.GetData<GetPlayerGamesDto>().Where(x => x.GameId == e.AggregateId).ToList();

            foreach (var g in games)
            {
                QueryDataStore.Delete<GetPlayerGamesDto>(g);
            }
        }

        public void Handle(PlayerRenamedEvent e)
        {
            var players = QueryDataStore.GetData<GetPlayerGamesDto>().Where(x => x.PlayerName == e.OldPlayerName).ToList();

            foreach (var p in players)
            {
                p.PlayerName = e.NewPlayerName;
            }

            QueryDataStore.SaveChanges();
        }
    }
}

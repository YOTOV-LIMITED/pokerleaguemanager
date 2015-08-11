using System.Linq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.DTO.DataTransferObjects.Lookups;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core.EventHandlers
{
    public class GetPlayerStatisticsHandler : BaseHandler, IHandlesEvent<PlayerAddedToGameEvent>, IHandlesEvent<GameDeletedEvent>, IHandlesEvent<PlayerRenamedEvent>
    {
        public void Handle(PlayerAddedToGameEvent e)
        {
            var player = QueryDataStore.GetData<GetPlayerStatisticsDto>().FirstOrDefault(x => x.PlayerName == e.PlayerName);

            if (player == null)
            {
                player = new GetPlayerStatisticsDto();

                AddGameToPlayer(player, e);

                QueryDataStore.Insert<GetPlayerStatisticsDto>(player);
            }
            else
            {
                AddGameToPlayer(player, e);

                QueryDataStore.SaveChanges();
            }
        }

        public void Handle(GameDeletedEvent e)
        {
            var players = QueryDataStore.GetData<LookupGamePlayersDto>().Where(x => x.GameId == e.AggregateId);

            foreach (var p in players)
            {
                var stats = QueryDataStore.GetData<GetPlayerStatisticsDto>().First(x => x.PlayerName == p.PlayerName);

                stats.GamesPlayed--;
                stats.Winnings -= p.Winnings;
                stats.PayIn -= p.PayIn;
                stats.Profit -= p.Winnings - p.PayIn;
                stats.ProfitPerGame = stats.Profit == 0 ? 0 : stats.Profit / stats.GamesPlayed;

                QueryDataStore.SaveChanges();
            }
        }

        public void Handle(PlayerRenamedEvent e)
        {
            var players = QueryDataStore.GetData<GetPlayerStatisticsDto>().Where(x => x.PlayerName == e.OldPlayerName).ToList();

            foreach (var p in players)
            {
                p.PlayerName = e.NewPlayerName;
            }

            QueryDataStore.SaveChanges();
        }

        private void AddGameToPlayer(GetPlayerStatisticsDto player, PlayerAddedToGameEvent e)
        {
            player.PlayerName = e.PlayerName;
            player.GamesPlayed++;
            player.Winnings += e.Winnings;
            player.PayIn += e.PayIn;
            player.Profit += e.Winnings - e.PayIn;
            player.ProfitPerGame = player.Profit / player.GamesPlayed;
        }
    }
}

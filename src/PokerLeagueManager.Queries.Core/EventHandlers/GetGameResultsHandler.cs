using System.Linq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core.EventHandlers
{
    public class GetGameResultsHandler : BaseHandler, IHandlesEvent<GameCreatedEvent>, IHandlesEvent<PlayerAddedToGameEvent>, IHandlesEvent<GameDeletedEvent>, IHandlesEvent<PlayerRenamedEvent>
    {
        public void Handle(GameCreatedEvent e)
        {
            QueryDataStore.Insert(new GetGameResultsDto()
            {
                GameId = e.AggregateId,
                GameDate = e.GameDate
            });
        }

        public void Handle(PlayerAddedToGameEvent e)
        {
            var game = QueryDataStore.GetData<GetGameResultsDto>().Single(x => x.GameId == e.AggregateId);

            game.Players.Add(new GetGameResultsDto.PlayerDto()
                {
                    PlayerName = e.PlayerName,
                    Placing = e.Placing,
                    Winnings = e.Winnings,
                    PayIn = e.PayIn
                });

            QueryDataStore.SaveChanges();
        }

        public void Handle(GameDeletedEvent e)
        {
            var dto = QueryDataStore.GetData<GetGameResultsDto>().Single(x => x.GameId == e.AggregateId);
            QueryDataStore.Delete<GetGameResultsDto>(dto);
        }

        public void Handle(PlayerRenamedEvent e)
        {
            var dto = QueryDataStore.GetData<GetGameResultsDto>().Single(x => x.GameId == e.AggregateId);

            var renamedPlayer = dto.Players.First(p => p.PlayerName == e.OldPlayerName);

            renamedPlayer.PlayerName = e.NewPlayerName;

            QueryDataStore.SaveChanges();
        }
    }
}

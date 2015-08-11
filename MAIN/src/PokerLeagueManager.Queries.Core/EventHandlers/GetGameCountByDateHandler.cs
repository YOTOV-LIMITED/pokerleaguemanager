using System.Linq;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core.EventHandlers
{
    public class GetGameCountByDateHandler : BaseHandler, IHandlesEvent<GameCreatedEvent>, IHandlesEvent<GameDeletedEvent>
    {
        public void Handle(GameCreatedEvent e)
        {
            QueryDataStore.Insert(new GetGameCountByDateDto()
            {
                GameId = e.AggregateId,
                GameYear = e.GameDate.Year,
                GameMonth = e.GameDate.Month,
                GameDay = e.GameDate.Day
            });
        }

        public void Handle(GameDeletedEvent e)
        {
            var dto = QueryDataStore.GetData<GetGameCountByDateDto>().Single(d => d.GameId == e.AggregateId);
            QueryDataStore.Delete<GetGameCountByDateDto>(dto);
        }
    }
}

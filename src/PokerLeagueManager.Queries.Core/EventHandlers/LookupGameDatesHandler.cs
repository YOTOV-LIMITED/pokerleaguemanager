using PokerLeagueManager.Common.DTO.DataTransferObjects.Lookups;
using PokerLeagueManager.Common.Events;
using PokerLeagueManager.Queries.Core.Infrastructure;

namespace PokerLeagueManager.Queries.Core.EventHandlers
{
    public class LookupGameDatesHandler : BaseHandler, IHandlesEvent<GameCreatedEvent>
    {
        public void Handle(GameCreatedEvent e)
        {
            var dto = new LookupGameDatesDto();

            dto.GameId = e.AggregateId;
            dto.GameDate = e.GameDate;

            QueryDataStore.Insert<LookupGameDatesDto>(dto);
        }
    }
}

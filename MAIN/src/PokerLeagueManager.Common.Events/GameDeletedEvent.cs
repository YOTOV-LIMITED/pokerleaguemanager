using System.Runtime.Serialization;
using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Common.Events
{
    [DataContract]
    public class GameDeletedEvent : BaseEvent
    {
        // no properties needed, GameId is stored in BaseEvent.AggregateId
    }
}

using System;
using System.Runtime.Serialization;
using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Common.Events
{
    [DataContract]
    public class GameCreatedEvent : BaseEvent
    {
        [DataMember]
        public DateTime GameDate { get; set; }
    }
}

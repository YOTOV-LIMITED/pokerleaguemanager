using System.Runtime.Serialization;
using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Common.Events
{
    [DataContract]
    public class PlayerAddedToGameEvent : BaseEvent
    {
        [DataMember]
        public string PlayerName { get; set; }

        [DataMember]
        public int Placing { get; set; }

        [DataMember]
        public int Winnings { get; set; }

        [DataMember]
        public int PayIn { get; set; }
    }
}

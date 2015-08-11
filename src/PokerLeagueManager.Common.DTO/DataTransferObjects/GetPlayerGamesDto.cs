using System;
using System.Runtime.Serialization;
using PokerLeagueManager.Common.DTO.Infrastructure;

namespace PokerLeagueManager.Common.DTO
{
    [DataContract]
    public class GetPlayerGamesDto : BaseDataTransferObject
    {
        [DataMember]
        public Guid GameId { get; set; }

        [DataMember]
        public string PlayerName { get; set; }

        [DataMember]
        public DateTime GameDate { get; set; }

        [DataMember]
        public int Placing { get; set; }

        [DataMember]
        public int Winnings { get; set; }

        [DataMember]
        public int PayIn { get; set; }
    }
}

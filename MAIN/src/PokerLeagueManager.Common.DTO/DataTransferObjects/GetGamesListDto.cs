using System;
using System.Runtime.Serialization;
using PokerLeagueManager.Common.DTO.Infrastructure;

namespace PokerLeagueManager.Common.DTO
{
    [DataContract]
    public class GetGamesListDto : BaseDataTransferObject
    {
        [DataMember]
        public Guid GameId { get; set; }

        [DataMember]
        public DateTime GameDate { get; set; }

        [DataMember]
        public int Winnings { get; set; }

        [DataMember]
        public string Winner { get; set; }
    }
}

using System;
using System.Runtime.Serialization;
using PokerLeagueManager.Common.DTO.Infrastructure;

namespace PokerLeagueManager.Common.DTO.DataTransferObjects.Lookups
{
    public class LookupGameDatesDto : BaseDataTransferObject
    {
        [DataMember]
        public Guid GameId { get; set; }

        [DataMember]
        public DateTime GameDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using PokerLeagueManager.Common.DTO.Infrastructure;

namespace PokerLeagueManager.Common.DTO
{
    [DataContract]
    public class GetGameResultsDto : BaseDataTransferObject
    {
        public GetGameResultsDto()
        {
            Players = new List<PlayerDto>();
        }

        [DataMember]
        public Guid GameId { get; set; }

        [DataMember]
        public DateTime GameDate { get; set; }

        [DataMember]
        public ICollection<PlayerDto> Players { get; private set; }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "By Design")]
        [DataContract]
        public class PlayerDto : BaseDataTransferObject
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
}

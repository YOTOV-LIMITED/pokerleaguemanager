using System.Runtime.Serialization;
using PokerLeagueManager.Common.DTO.Infrastructure;

namespace PokerLeagueManager.Common.DTO
{
    [DataContract]
    public class GetPlayerStatisticsDto : BaseDataTransferObject
    {
        [DataMember]
        public string PlayerName { get; set; }

        [DataMember]
        public int GamesPlayed { get; set; }

        [DataMember]
        public int Winnings { get; set; }

        [DataMember]
        public int PayIn { get; set; }

        [DataMember]
        public int Profit { get; set; }

        [DataMember]
        public double ProfitPerGame { get; set; }
    }
}

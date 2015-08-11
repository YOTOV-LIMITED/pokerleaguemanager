using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using PokerLeagueManager.Common.Commands.Infrastructure;

namespace PokerLeagueManager.Common.Commands
{
    [DataContract]
    public class EnterGameResultsCommand : BaseCommand
    {
        [DataMember]
        public Guid GameId { get; set; }

        [DataMember]
        public DateTime GameDate { get; set; }

        [DataMember]
        public IEnumerable<GamePlayer> Players { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "By Design")]
        [DataContract]
        public class GamePlayer
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

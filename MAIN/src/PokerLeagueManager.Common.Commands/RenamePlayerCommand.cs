using System.Runtime.Serialization;
using PokerLeagueManager.Common.Commands.Infrastructure;

namespace PokerLeagueManager.Common.Commands
{
    [DataContract]
    public class RenamePlayerCommand : BaseCommand
    {
        [DataMember]
        public string OldPlayerName { get; set; }

        [DataMember]
        public string NewPlayerName { get; set; }
    }
}

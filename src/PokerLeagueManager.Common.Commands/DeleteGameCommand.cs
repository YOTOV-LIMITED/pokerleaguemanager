using System;
using System.Runtime.Serialization;
using PokerLeagueManager.Common.Commands.Infrastructure;

namespace PokerLeagueManager.Common.Commands
{
    [DataContract]
    public class DeleteGameCommand : BaseCommand
    {
        [DataMember]
        public Guid GameId { get; set; }
    }
}

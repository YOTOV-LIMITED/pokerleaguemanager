using System;
using System.Runtime.Serialization;

namespace PokerLeagueManager.Commands.Domain.Exceptions
{
    [Serializable]
    public class PlayerNameMustNotBeBlankException : Exception
    {
        public PlayerNameMustNotBeBlankException()
            : base("Player Name must not be blank")
        {
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace PokerLeagueManager.Commands.Domain.Exceptions
{
    [Serializable]
    public class PlayerPlacingsNotInOrderException : Exception
    {
        public PlayerPlacingsNotInOrderException()
            : base("The player placings must start at one and have no duplicates and not be higher than the total # of players")
        {
        }
    }
}

using System;

namespace PokerLeagueManager.Commands.Domain.Exceptions
{
    [Serializable]
    public class WinningsDoesNotEqualPayInsException : Exception
    {
        public WinningsDoesNotEqualPayInsException()
            : base("The total player Winnings must be equal to the total player Pay Ins")
        {
        }
    }
}

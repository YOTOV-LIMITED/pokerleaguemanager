using System;
using System.Runtime.Serialization;

namespace PokerLeagueManager.Commands.Domain.Exceptions
{
    [Serializable]
    public class WinningsCannotBeNegativeException : Exception
    {
        public WinningsCannotBeNegativeException(int winnings, string playerName)
            : base(CreateMessage(winnings, playerName))
        {
        }

        private static string CreateMessage(int winnings, string playerName)
        {
            var msg = "Winnings cannot be negative";
            msg += Environment.NewLine;
            msg += Environment.NewLine;
            msg += "Player Name: " + playerName;
            msg += Environment.NewLine;
            msg += "Winnings: " + winnings.ToString();

            return msg;
        }
    }
}

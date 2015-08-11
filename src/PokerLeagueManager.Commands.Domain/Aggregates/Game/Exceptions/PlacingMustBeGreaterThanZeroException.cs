using System;
using System.Runtime.Serialization;

namespace PokerLeagueManager.Commands.Domain.Exceptions
{
    [Serializable]
    public class PlacingMustBeGreaterThanZeroException : Exception
    {
        public PlacingMustBeGreaterThanZeroException(int placing, string playerName)
            : base(CreateMessage(placing, playerName))
        {
        }

        private static string CreateMessage(int placing, string playerName)
        {
            var msg = "Placing must be greater than 0";
            msg += Environment.NewLine;
            msg += Environment.NewLine;
            msg += "Player Name: " + playerName;
            msg += Environment.NewLine;
            msg += "Placing: " + placing;

            return msg;
        }
    }
}

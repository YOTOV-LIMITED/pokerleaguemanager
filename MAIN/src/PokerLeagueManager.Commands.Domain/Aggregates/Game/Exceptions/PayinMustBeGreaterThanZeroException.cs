using System;

namespace PokerLeagueManager.Commands.Domain.Exceptions
{
    [Serializable]
    public class PayinMustBeGreaterThanZeroException : Exception
    {
        public PayinMustBeGreaterThanZeroException(int payin, string playerName)
            : base(CreateMessage(payin, playerName))
        {
        }

        private static string CreateMessage(int payin, string playerName)
        {
            var msg = "Payin must be greater than 0";
            msg += Environment.NewLine;
            msg += Environment.NewLine;
            msg += "Player Name: " + playerName;
            msg += Environment.NewLine;
            msg += "Payin: " + payin;

            return msg;
        }
    }
}

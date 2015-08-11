using System;
using System.Runtime.Serialization;

namespace PokerLeagueManager.Commands.Domain.Exceptions
{
    [Serializable]
    public class DuplicatePlayerNameException : Exception
    {
        public DuplicatePlayerNameException(string playerName)
            : base(CreateMessage(playerName))
        {
        }

        private static string CreateMessage(string playerName)
        {
            var msg = "Cannot add the same Player to a Game more than once";
            msg += Environment.NewLine;
            msg += Environment.NewLine;
            msg += "Player Name: " + playerName;

            return msg;
        }
    }
}

using System;
using PokerLeagueManager.Commands.Domain.Infrastructure;
using PokerLeagueManager.Common.Commands.Infrastructure;

namespace PokerLeagueManager.Commands.Domain.Infrastructure.Exceptions
{
    [Serializable]
    public class PublishEventFailedException : Exception
    {
        public PublishEventFailedException(IAggregateRoot aggRoot, ICommand c, Exception ex)
            : base(CreateMessage(aggRoot, c, ex))
        {
        }

        private static string CreateMessage(IAggregateRoot aggRoot, ICommand c, Exception ex)
        {
            var msg = "The action you were trying to perform succeeded, however not all systems have been updated yet.";
            msg += Environment.NewLine;
            msg += Environment.NewLine;
            msg += "Aggregate ID: " + aggRoot.AggregateId.ToString();
            msg += Environment.NewLine;
            msg += "Command ID: " + c.CommandId.ToString();
            msg += Environment.NewLine;
            msg += "Exception: " + ex.GetType().Name + " - " + ex.Message;

            return msg;
        }
    }
}

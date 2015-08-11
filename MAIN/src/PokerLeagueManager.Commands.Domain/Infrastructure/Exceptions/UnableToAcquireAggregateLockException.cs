using System;

namespace PokerLeagueManager.Commands.Domain.Infrastructure.Exceptions
{
    [Serializable]
    public class UnableToAcquireAggregateLockException : Exception
    {
        public UnableToAcquireAggregateLockException(Guid aggregateId, Exception ex)
            : base(CreateMessage(aggregateId, ex), ex)
        {
        }

        private static string CreateMessage(Guid aggregateId, Exception ex)
        {
            var msg = "The entity you were trying to save is currently being updated by another user, you will have to start the editing process again to retry.";
            msg += Environment.NewLine;
            msg += Environment.NewLine;
            msg += "Aggregate ID: " + aggregateId.ToString();
            msg += Environment.NewLine;
            msg += "Exception: " + ex.GetType().Name + " - " + ex.Message;

            return msg;
        }
    }
}

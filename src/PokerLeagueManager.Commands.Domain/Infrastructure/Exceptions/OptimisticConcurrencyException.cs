using System;
using PokerLeagueManager.Commands.Domain.Infrastructure;

namespace PokerLeagueManager.Commands.Domain.Infrastructure.Exceptions
{
    [Serializable]
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException(IAggregateRoot aggRoot, Guid originalVersion) : this(aggRoot, originalVersion, aggRoot.AggregateVersion)
        {
        }

        public OptimisticConcurrencyException(IAggregateRoot aggRoot, Guid originalVersion, Guid currentVersion) : base(CreateMessage(aggRoot, originalVersion, currentVersion))
        {
        }

        private static string CreateMessage(IAggregateRoot aggRoot, Guid originalVersion, Guid currentVersion)
        {
            var aggType = aggRoot.GetType().Name;

            var msg = string.Format("The {0} you were trying to save has been updated by another user, you will have to start the editing process again to retry.", aggType);
            msg += Environment.NewLine;
            msg += Environment.NewLine;
            msg += "Aggregate ID: " + aggRoot.AggregateId.ToString();
            msg += Environment.NewLine;
            msg += "Original Version: " + originalVersion.ToString();
            msg += Environment.NewLine;
            msg += "Current Version: " + currentVersion.ToString();

            return msg;
        }
    }
}

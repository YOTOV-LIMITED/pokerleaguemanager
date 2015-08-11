using System;
using System.Collections.Generic;
using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Commands.Domain.Infrastructure
{
    public interface IAggregateRoot
    {
        ICollection<IEvent> PendingEvents { get; }

        Guid AggregateId { get; set; }

        Guid AggregateVersion { get; set; }

        void ApplyEvent(IEvent e);
    }
}

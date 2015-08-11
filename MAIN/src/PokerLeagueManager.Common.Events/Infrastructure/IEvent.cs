using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Events.Infrastructure
{
    public interface IEvent
    {
        Guid EventId { get; set; }

        DateTime Timestamp { get; set; }

        Guid CommandId { get; set; }

        Guid AggregateId { get; set; }
    }
}

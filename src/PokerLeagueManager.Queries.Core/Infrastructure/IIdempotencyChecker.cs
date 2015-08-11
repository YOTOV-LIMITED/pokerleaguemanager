using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Queries.Core.Infrastructure
{
    public interface IIdempotencyChecker
    {
        IDatabaseLayer DatabaseLayer { get; set; }

        bool CheckIdempotency(Guid eventId);

        void MarkEventAsProcessed(Guid eventId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Queries.Core.Infrastructure
{
    public class IdempotencyChecker : IIdempotencyChecker
    {
        private IDateTimeService _dateTimeService;

        public IdempotencyChecker(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public IDatabaseLayer DatabaseLayer { get; set; }

        public bool CheckIdempotency(Guid eventId)
        {
            var eventCount = (int)DatabaseLayer.ExecuteScalar("SELECT COUNT(*) FROM EventsProcessed WHERE EventId = @EventId", "@EventId", eventId);

            return eventCount > 0;
        }

        public void MarkEventAsProcessed(Guid eventId)
        {
            DatabaseLayer.ExecuteNonQuery("INSERT INTO EventsProcessed(EventId, ProcessedDateTime) VALUES(@EventId, @ProcessingDateTime)", "@EventId", eventId, "@ProcessingDateTime", _dateTimeService.UtcNow());
        }
    }
}

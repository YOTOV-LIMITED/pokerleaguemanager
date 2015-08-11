using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Events.Infrastructure
{
    [DataContract]
    public class BaseEvent : IEvent
    {
        public BaseEvent()
        {
            EventId = Guid.NewGuid();
            Timestamp = DateTime.Now;
            CommandId = Guid.Empty;
            AggregateId = Guid.Empty;
        }

        [DataMember]
        public Guid EventId { get; set; }

        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public Guid CommandId { get; set; }

        [DataMember]
        public Guid AggregateId { get; set; }
    }
}

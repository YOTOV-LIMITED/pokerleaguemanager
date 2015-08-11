using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Events.Infrastructure
{
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes", typeof(EventTypeProvider))]
    public interface IEventService
    {
        [OperationContract]
        void HandleEvent(IEvent e);
    }
}

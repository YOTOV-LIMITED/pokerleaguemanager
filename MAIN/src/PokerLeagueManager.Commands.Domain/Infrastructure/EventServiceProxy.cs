using System.ServiceModel;
using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Commands.Domain.Infrastructure
{
    public class EventServiceProxy : ClientBase<IEventService>, IEventService, IEventServiceProxy
    {
        public string ServiceUrl
        {
            get
            {
                return base.Endpoint.Address.Uri.ToString();
            }

            set
            {
                base.Endpoint.Address = new EndpointAddress(value);
            }
        }

        public void HandleEvent(IEvent e)
        {
            base.Channel.HandleEvent(e);
        }
    }
}

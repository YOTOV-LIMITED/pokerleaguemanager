using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Queries.Core.Infrastructure
{
    public interface IEventHandlerFactory
    {
        void HandleEvent(IEvent e);

        void HandleEvent<T>(T e) where T : IEvent;
    }
}

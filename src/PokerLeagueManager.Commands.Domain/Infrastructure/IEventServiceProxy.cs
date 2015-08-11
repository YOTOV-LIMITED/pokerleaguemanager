using PokerLeagueManager.Common.Events.Infrastructure;

namespace PokerLeagueManager.Commands.Domain.Infrastructure
{
    public interface IEventServiceProxy : IEventService
    {
        string ServiceUrl { get; set; }
    }
}

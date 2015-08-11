using Microsoft.Practices.Unity;
using PokerLeagueManager.Common.DTO;
using PokerLeagueManager.Common.Events.Infrastructure;
using PokerLeagueManager.Common.Utilities;

namespace PokerLeagueManager.Commands.Domain.Infrastructure
{
    public static class Bootstrapper
    {
        private static bool _hasBootstrapped = false;

        public static void Bootstrap()
        {
            if (!_hasBootstrapped)
            {
                UnitySingleton.Container.RegisterType<ICommandHandlerFactory, CommandHandlerFactory>();
                UnitySingleton.Container.RegisterType<IEventService, EventServiceProxy>();
                UnitySingleton.Container.RegisterType<IEventRepository, EventRepository>();
                UnitySingleton.Container.RegisterType<IEventServiceProxyFactory, EventServiceProxyFactory>();
                UnitySingleton.Container.RegisterType<ICommandRepository, CommandRepository>();

                PokerLeagueManager.Common.Commands.Infrastructure.Bootstrapper.Bootstrap();
                PokerLeagueManager.Common.DTO.Infrastructure.Bootstrapper.Bootstrap();
                PokerLeagueManager.Common.Events.Infrastructure.Bootstrapper.Bootstrap();
                PokerLeagueManager.Common.Utilities.Bootstrapper.Bootstrap();

                _hasBootstrapped = true;
            }
        }
    }
}
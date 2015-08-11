using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using PokerLeagueManager.Commands.Domain.Infrastructure;

namespace PokerLeagueManager.Utilities.ProcessEvents
{
    public static class Program
    {
        public static void Main()
        {
            Resolver.Container.Resolve<IEventRepository>().PublishAllUnpublishedEvents();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Commands.Infrastructure
{
    public class CommandServiceProxy : ClientBase<ICommandService>, ICommandService
    {
        public void ExecuteCommand(ICommand command)
        {
            base.Channel.ExecuteCommand(command);
        }
    }
}

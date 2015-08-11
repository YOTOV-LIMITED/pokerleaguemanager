using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Commands.Infrastructure
{
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes", typeof(CommandTypeProvider))]
    public interface ICommandService
    {
        [OperationContract]
        void ExecuteCommand(ICommand command);
    }
}

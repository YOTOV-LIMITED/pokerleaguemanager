using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerLeagueManager.Common.Commands.Infrastructure;

namespace PokerLeagueManager.Commands.Domain.Infrastructure
{
    public interface ICommandRepository
    {
        void LogCommand(ICommand command);

        void LogFailedCommand(ICommand command, Exception ex);
    }
}

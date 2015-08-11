using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerLeagueManager.Common.Commands.Infrastructure
{
    public interface ICommandFactory
    {
        T Create<T>() where T : ICommand, new();

        T Create<T>(T cmd) where T : ICommand;
    }
}

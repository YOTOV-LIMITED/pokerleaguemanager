using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PokerLeagueManager.Commands.Domain.Infrastructure
{
    public interface IEventServiceProxyFactory
    {
        IEventServiceProxy Create(DataRow row);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Commands.Infrastructure
{
    public interface ICommand
    {
        Guid CommandId { get; set; }

        DateTime Timestamp { get; set; }

        string User { get; set; }

        string IPAddress { get; set; }
    }
}

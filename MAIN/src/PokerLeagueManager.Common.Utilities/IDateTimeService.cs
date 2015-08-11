using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Utilities
{
    public interface IDateTimeService
    {
        DateTime Now();

        DateTime UtcNow();
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.DTO.Infrastructure
{
    public interface IDtoFactory
    {
        T Create<T>(DataRow row) where T : IDataTransferObject;
    }
}

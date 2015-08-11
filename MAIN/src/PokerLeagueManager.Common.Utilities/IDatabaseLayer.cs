using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Utilities
{
    public interface IDatabaseLayer
    {
        string ConnectionString { get; set; }

        int ExecuteNonQuery(string sql);

        int ExecuteNonQuery(string sql, params object[] sqlArgs);

        object ExecuteScalar(string sql);

        object ExecuteScalar(string sql, params object[] sqlArgs);

        DataTable GetDataTable(string sql);

        DataTable GetDataTable(string sql, params object[] sqlArgs);

        void ExecuteInTransaction(Action work);
    }
}

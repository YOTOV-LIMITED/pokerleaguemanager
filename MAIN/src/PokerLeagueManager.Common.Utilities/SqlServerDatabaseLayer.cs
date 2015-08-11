using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLeagueManager.Common.Utilities
{
    public class SqlServerDatabaseLayer : IDatabaseLayer, IDisposable
    {
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool _disposedValue;
        private string _connectionString;

        public SqlServerDatabaseLayer()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        ~SqlServerDatabaseLayer()
        {
            Dispose(false);
        }

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }

            set
            {
                _connectionString = value;
                _connection = new SqlConnection(_connectionString);
            }
        }

        public DataTable GetDataTable(string sql)
        {
            return GetDataTable(sql, new object[0]);
        }

        public DataTable GetDataTable(string sql, params object[] sqlArgs)
        {
            var myCommand = PrepareCommand(sql, sqlArgs);

            var result = new DataTable();

            try
            {
                using (var myAdapter = new SqlDataAdapter(myCommand))
                {
                    ExecuteWithConnection(() => myAdapter.Fill(result));
                }

                return result;
            }
            catch
            {
                result.Dispose();
                throw;
            }
        }

        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, new object[0]);
        }

        public int ExecuteNonQuery(string sql, params object[] sqlArgs)
        {
            var myCommand = PrepareCommand(sql, sqlArgs);
            int result = default(int);

            ExecuteWithConnection(() => result = myCommand.ExecuteNonQuery());

            return result;
        }

        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, new object[0]);
        }

        public object ExecuteScalar(string sql, params object[] sqlArgs)
        {
            var myCommand = this.PrepareCommand(sql, sqlArgs);
            object result = default(object);

            ExecuteWithConnection(() => result = myCommand.ExecuteScalar());

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ExecuteInTransaction(Action work)
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            try
            {
                work();
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _connection.Close();

                _transaction.Dispose();
                _transaction = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        if (_connection.State == ConnectionState.Open)
                        {
                            _connection.Close();
                        }

                        _connection.Dispose();
                        _transaction.Dispose();
                    }
                }
            }

            _disposedValue = true;
        }

        private void CloseConnection()
        {
            if (_transaction == null)
            {
                _connection.Close();
            }
        }

        private void OpenConnection()
        {
            if (_transaction == null)
            {
                _connection.Open();
            }
        }

        private void ExecuteWithConnection(Action work)
        {
            OpenConnection();

            try
            {
                work();
            }
            finally
            {
                CloseConnection();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "It will be the responsibility of the caller to ensure they aren't vulnerable to SQL Injection")]
        private SqlCommand PrepareCommand(string sql, params object[] sqlArgs)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("The SQL statement was blank. A valid SQL Statement must be provided.", "sql");
            }

            var myCommand = new SqlCommand(sql, _connection);

            try
            {
                myCommand.CommandTimeout = 0;
                myCommand.CommandType = CommandType.Text;

                for (int i = 0; i < sqlArgs.Length; i += 2)
                {
                    myCommand.Parameters.AddWithValue((string)sqlArgs[i], sqlArgs[i + 1]);
                }

                myCommand.Transaction = _transaction;

                return myCommand;
            }
            catch
            {
                myCommand.Dispose();
                throw;
            }
        }
    }
}

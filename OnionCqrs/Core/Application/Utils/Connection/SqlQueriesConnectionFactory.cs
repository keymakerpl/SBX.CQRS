using System;
using System.Data;
using System.Data.SqlClient;

namespace Application.Utils.Connection
{
    public class SqlQueriesConnectionFactory : ISqlQueriesConnectionFactory, IDisposable
    {
        private readonly string queriesConnectionString;
        private IDbConnection connection;
        private bool disposedValue;

        public SqlQueriesConnectionFactory(QueriesConnectionString queriesConnectionString) =>
            this.queriesConnectionString = queriesConnectionString.Value;

        public IDbConnection CreateOpenConnection()
        {
            if (this.connection == null || this.connection.State != ConnectionState.Open)
            {
                this.connection = new SqlConnection(queriesConnectionString);
                this.connection.Open();
            }

            return this.connection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.connection != null && this.connection.State == ConnectionState.Open)
                    {
                        this.connection.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

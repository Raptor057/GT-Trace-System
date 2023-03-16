using Microsoft.Data.SqlClient;
using System.Data;

namespace GT.Trace.Common.Infra.DataSources.SqlDB.Implementations
{
    /// <summary>
    /// Database connection factory that requires a connection string.
    /// </summary>
    internal class ConnectionStringSqlDbConnectionFactory : ISqlDbConnectionFactory
    {
        /// <summary>
        /// Database connection string.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Set the database connection string to use with the parameter value.
        /// </summary>
        public ConnectionStringSqlDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Initialize a new instance of the IDbConnection asynchronously and open
        /// it before returning it.
        /// </summary>
        public async Task<IDbConnection> CreateOpenConnectionAsync()
        {
            var con = new SqlConnection(_connectionString);
            await con.OpenAsync().ConfigureAwait(false);
            return con;
        }

        /// <summary>
        /// Initialize a new instance of the IDbConnection synchronously and open
        /// it before returning it.
        /// </summary>
        public IDbConnection CreateOpenConnection() => CreateOpenConnectionAsync().GetAwaiter().GetResult();
    }
}
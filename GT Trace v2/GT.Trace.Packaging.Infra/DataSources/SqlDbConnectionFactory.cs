using Microsoft.Data.SqlClient;
using System.Data;

namespace GT.Trace.Packaging.Infra.DataSources
{
    public class SqlDbConnectionFactory
    {
        protected readonly string _connectionString;

        public SqlDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> GetOpenConnectionAsync()
        {
            var con = new SqlConnection(_connectionString);
            await con.OpenAsync().ConfigureAwait(false);
            return con;
        }
    }
}
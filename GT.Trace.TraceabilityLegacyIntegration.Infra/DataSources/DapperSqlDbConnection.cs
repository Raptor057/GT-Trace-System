/*Tercer archivo creado para DataSources*/
using Dapper;

namespace GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources
{
    public class DapperSqlDbConnection
    {
        protected readonly SqlDbConnectionFactory _connections;

        public DapperSqlDbConnection(SqlDbConnectionFactory connections)
        {
            _connections = connections;
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.ExecuteAsync(sql, param).ConfigureAwait(false);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object? param = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.ExecuteScalarAsync<T>(sql, param).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.QueryAsync<T>(sql, param).ConfigureAwait(false);
        }

        public async Task<T> QuerySingleAsync<T>(string query, object? param = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.QuerySingleOrDefaultAsync<T>(query, param).ConfigureAwait(false);
        }

        public async Task<T> QueriFirsrAsync<T>(string sql, object? param = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.QueryFirstOrDefaultAsync<T>(sql, param).ConfigureAwait(false);
        }

    }
}

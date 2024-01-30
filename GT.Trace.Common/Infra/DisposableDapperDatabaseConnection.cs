using Dapper;
using System.Data;

namespace GT.Trace.Common.Infra
{
    public class DisposableDapperDatabaseConnection<TSqlDatabaseConnectionFactory> : ISqlDatabaseConnection
        where TSqlDatabaseConnectionFactory : ISqlDatabaseConnectionFactory
    {
        private readonly ISqlDatabaseConnectionFactory _connections;

        public DisposableDapperDatabaseConnection(TSqlDatabaseConnectionFactory connections)
        {
            _connections = connections;
        }
#pragma warning disable CS8603 // Possible null reference return.
        public async Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object? @params = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.QueryAsync<T>(sqlCommand, @params)
                .ConfigureAwait(false);
        }

        public async Task<T> QuerySingleAsync<T>(string sqlCommand, object? @params = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);

            return await con.QuerySingleOrDefaultAsync<T>(sqlCommand, @params)
                .ConfigureAwait(false);

        }

        public async Task<T> QueryFirstAsync<T>(string sqlCommand, object? @params = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.QueryFirstOrDefaultAsync<T>(sqlCommand, @params)
                .ConfigureAwait(false);
        }

        public async Task<int> ExecuteAsync(string sqlCommand, object? @params = null, CommandType commandType = CommandType.Text)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.ExecuteAsync(sqlCommand, @params, commandType: commandType)
                .ConfigureAwait(false);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sqlCommand, object? @params = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.ExecuteScalarAsync<T>(sqlCommand, @params)
                .ConfigureAwait(false);
        }

        public IEnumerable<T> Query<T>(string sqlCommand, object? @params = null) => QueryAsync<T>(sqlCommand, @params).GetAwaiter().GetResult();

        public T QuerySingle<T>(string sqlCommand, object? @params = null) => QuerySingleAsync<T>(sqlCommand, @params).GetAwaiter().GetResult();

        public T QueryFirst<T>(string sqlCommand, object? @params = null) => QueryFirstAsync<T>(sqlCommand, @params).GetAwaiter().GetResult();

        public int Execute(string sqlCommand, object? @params = null, CommandType commandType = CommandType.Text) => ExecuteAsync(sqlCommand, @params, commandType).GetAwaiter().GetResult();

        public T ExecuteScalar<T>(string sqlCommand, object? @params = null) => ExecuteScalarAsync<T>(sqlCommand, @params).GetAwaiter().GetResult();
    }
}
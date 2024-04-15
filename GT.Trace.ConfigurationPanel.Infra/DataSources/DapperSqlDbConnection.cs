//using Dapper;
//using System.Data;
//using Microsoft.Extensions.Logging;

//namespace GT.Trace.ConfigurationPanel.Infra.DataSources
//{
//    public class DapperSqlDbConnection
//    {
//        protected readonly SqlDbConnectionFactory _connections;
//        protected readonly ILogger _logger;

//        public DapperSqlDbConnection(SqlDbConnectionFactory connections, ILogger logger)
//        {
//            _connections = connections;
//            _logger = logger;
//        }

//        public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null)
//        {
//            try
//            {
//                using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
//                return await con.ExecuteAsync(sql, param, transaction).ConfigureAwait(false);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error executing SQL: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<T> ExecuteScalarAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
//        {
//            try
//            {
//                using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
//                return await con.ExecuteScalarAsync<T>(sql, param, transaction).ConfigureAwait(false);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error executing scalar SQL: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
//        {
//            try
//            {
//                using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
//                return await con.QueryAsync<T>(sql, param, transaction).ConfigureAwait(false);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error executing query SQL: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
//        {
//            try
//            {
//                using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
//                return await con.QuerySingleOrDefaultAsync<T>(sql, param, transaction).ConfigureAwait(false);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error executing single query SQL: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<T> QueryFirstAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
//        {
//            try
//            {
//                using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
//                return await con.QueryFirstOrDefaultAsync<T>(sql, param, transaction).ConfigureAwait(false);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error executing first query SQL: {ex.Message}");
//                throw;
//            }
//        }

//        public async Task<int> ExecuteWithTransactionAsync(Func<IDbTransaction, Task<int>> action)
//        {
//            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
//            using var transaction = con.BeginTransaction();
//            try
//            {
//                var result = await action(transaction).ConfigureAwait(false);
//                transaction.Commit();
//                return result;
//            }
//            catch (Exception ex)
//            {
//                transaction.Rollback();
//                _logger.LogError($"Transaction failed: {ex.Message}");
//                throw;
//            }
//        }
//    }
//}

//----------------
using Dapper;

namespace GT.Trace.ConfigurationPanel.Infra.DataSources
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

        public async Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.ExecuteScalarAsync<T>(sql, param).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.QueryAsync<T>(sql, param).ConfigureAwait(false);
        }

        public async Task<T?> QuerySingleAsync<T>(string sql, object? param = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.QuerySingleOrDefaultAsync<T>(sql, param).ConfigureAwait(false);
        }

        public async Task<T?> QueryFirstAsync<T>(string sql, object? param = null)
        {
            using var con = await _connections.GetOpenConnectionAsync().ConfigureAwait(false);
            return await con.QueryFirstOrDefaultAsync<T>(sql, param).ConfigureAwait(false);
        }
    }
}

using System.Data;

namespace GT.Trace.Common.Infra
{
    public interface ISqlDatabaseConnection
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object? @params = null);

        Task<T> QuerySingleAsync<T>(string sqlCommand, object? @params = null);

        Task<T> QueryFirstAsync<T>(string sqlCommand, object? @params = null);

        Task<int> ExecuteAsync(string sqlCommand, object? @params = null, CommandType commandType = CommandType.Text);

        Task<T> ExecuteScalarAsync<T>(string sqlCommand, object? @params = null);

        IEnumerable<T> Query<T>(string sqlCommand, object? @params = null);

        T QuerySingle<T>(string sqlCommand, object? @params = null);

        T QueryFirst<T>(string sqlCommand, object? @params = null);

        int Execute(string sqlCommand, object? @params = null, CommandType commandType = CommandType.Text);

        T ExecuteScalar<T>(string sqlCommand, object? @params = null);
    }
}
using System.Data;

namespace GT.Trace.Infra.DataSources.Sql
{
    public interface ISqlDatabaseConnectionFactory
    {
        Task<IDbConnection> GetOpenConnectionAsync();

        IDbConnection GetOpenConnection();
    }
}
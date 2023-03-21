using System.Data;

namespace GT.Trace.Common.Infra
{
    public interface ISqlDatabaseConnectionFactory
    {
        Task<IDbConnection> GetOpenConnectionAsync();

        IDbConnection GetOpenConnection();
    }
}
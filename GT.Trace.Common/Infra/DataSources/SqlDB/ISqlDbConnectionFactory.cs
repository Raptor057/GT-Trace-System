using System.Data;

namespace GT.Trace.Common.Infra.DataSources.SqlDB
{
    /// <summary>
    /// Database connection factory.
    /// </summary>
    public interface ISqlDbConnectionFactory
    {
        /// <summary>
        /// Return an open instance of the IDbConnection.
        /// </summary>
        Task<IDbConnection> CreateOpenConnectionAsync();

        /// <summary>
        /// Return an open instance of the IDbConnection.
        /// </summary>
        IDbConnection CreateOpenConnection();
    }
}
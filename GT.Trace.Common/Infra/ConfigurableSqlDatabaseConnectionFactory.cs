using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GT.Trace.Common.Infra
{
    public class ConfigurableSqlDatabaseConnectionFactory : ISqlDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public ConfigurableSqlDatabaseConnectionFactory(IConfigurationRoot configuration, string? connectionStringName)
        {
            connectionStringName ??= GetType().Name;
            _connectionString = configuration.GetConnectionString(connectionStringName) ?? "";
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new NullReferenceException($"La cadena de conexión \"{connectionStringName}\" no se encuentra en el archivo configuración o se encuentra en blanco.");
            }
        }

        public async Task<IDbConnection> GetOpenConnectionAsync()
        {
            var con = new SqlConnection(_connectionString);
            await con.OpenAsync().ConfigureAwait(false);
            return con;
        }

        public IDbConnection GetOpenConnection() => GetOpenConnectionAsync().GetAwaiter().GetResult();
    }
}
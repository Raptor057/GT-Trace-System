using Microsoft.Extensions.Configuration;

namespace GT.Trace.Common.Infra.DataSources.SqlDB.Implementations
{
    /// <summary>
    /// Database connection factory that requires a connection string to be stored
    /// in settings file with a name matching the generic parameter type name.
    /// </summary>
    internal class GenericConfigurationSqlDbConnectionFactory<T> : ConnectionStringSqlDbConnectionFactory
    {
        /// <summary>
        /// Get the connection string from the configuration file named after the generic parameter type
        /// and passes it to the base class.
        /// </summary>
        public GenericConfigurationSqlDbConnectionFactory(IConfigurationRoot config)
            : base(config.GetConnectionString(typeof(T).Name) ?? throw new KeyNotFoundException($"Cadena de conexión \"{typeof(T).Name}\" no encontrada o en blanco."))
        { }
    }
}
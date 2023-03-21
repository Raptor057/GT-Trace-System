using Microsoft.Extensions.Configuration;

namespace GT.Trace.Infra.DataSources.Sql
{
    internal class TrazaConfigurableSqlDatabaseConnectionFactory : ConfigurableSqlDatabaseConnectionFactory
    {
        public TrazaConfigurableSqlDatabaseConnectionFactory(IConfigurationRoot configuration)
            : base(configuration, "TRAZAB")
        { }
    }
}
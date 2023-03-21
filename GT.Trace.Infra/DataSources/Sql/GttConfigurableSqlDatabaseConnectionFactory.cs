using Microsoft.Extensions.Configuration;

namespace GT.Trace.Infra.DataSources.Sql
{
    internal class GttConfigurableSqlDatabaseConnectionFactory : ConfigurableSqlDatabaseConnectionFactory
    {
        public GttConfigurableSqlDatabaseConnectionFactory(IConfigurationRoot configuration)
            : base(configuration, "GTT")
        { }
    }
}
using Microsoft.Extensions.Configuration;

namespace GT.Trace.Infra.DataSources.Sql
{
    internal class AppsConfigurableSqlDatabaseConnectionFactory : ConfigurableSqlDatabaseConnectionFactory
    {
        public AppsConfigurableSqlDatabaseConnectionFactory(IConfigurationRoot configuration)
            : base(configuration, "APPS")
        { }
    }
}
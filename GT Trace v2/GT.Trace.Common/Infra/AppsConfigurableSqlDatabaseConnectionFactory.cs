using Microsoft.Extensions.Configuration;

namespace GT.Trace.Common.Infra
{
    public class AppsConfigurableSqlDatabaseConnectionFactory : ConfigurableSqlDatabaseConnectionFactory
    {
        public AppsConfigurableSqlDatabaseConnectionFactory(IConfigurationRoot configuration)
            : base(configuration, "APPS")
        { }
    }
}
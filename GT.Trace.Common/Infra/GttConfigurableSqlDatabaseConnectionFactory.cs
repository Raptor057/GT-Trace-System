using Microsoft.Extensions.Configuration;

namespace GT.Trace.Common.Infra
{
    public class GttConfigurableSqlDatabaseConnectionFactory : ConfigurableSqlDatabaseConnectionFactory
    {
        public GttConfigurableSqlDatabaseConnectionFactory(IConfigurationRoot configuration)
            : base(configuration, "GTT")
        { }
    }
}
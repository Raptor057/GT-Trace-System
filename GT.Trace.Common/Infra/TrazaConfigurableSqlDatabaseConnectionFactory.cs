using Microsoft.Extensions.Configuration;

namespace GT.Trace.Common.Infra
{
    public class TrazaConfigurableSqlDatabaseConnectionFactory : ConfigurableSqlDatabaseConnectionFactory
    {
        public TrazaConfigurableSqlDatabaseConnectionFactory(IConfigurationRoot configuration)
            : base(configuration, "TRAZAB")
        { }
    }
}
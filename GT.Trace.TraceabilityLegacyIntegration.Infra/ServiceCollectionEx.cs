using GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GT.Trace.Common.Logging;


namespace GT.Trace.TraceabilityLegacyIntegration.Infra
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddLoggingServices(configuration)
                .AddSingleton(typeof(ConfigurationSqlDbConnectionFactory<>))
                .AddSingleton(typeof(ConfigurationSqlDbConnection<>))
                .AddSingleton<AppsSqlDB>()
                .AddSingleton<TrazaSqlDB>()
                .AddSingleton<CegidSqlDB>();
        }
    }
}

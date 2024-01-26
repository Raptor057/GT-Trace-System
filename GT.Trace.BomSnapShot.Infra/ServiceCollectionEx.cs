using GT.Trace.Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GT.Trace.BomSnapShot.Infra.DataSources;
using GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot;
using GT.Trace.BomSnapShot.Infra.Gateways;

namespace GT.Trace.BomSnapShot.Infra
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddSnapshotInfraServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddLoggingServices(configuration)
                .AddSingleton(typeof(ConfigurationSqlDbConnectionFactory<>))
                .AddSingleton(typeof(ConfigurationSqlDbConnection<>))
                .AddSingleton<AppsSqlDB>()
                .AddSingleton<TrazaSqlDB>()
                .AddSingleton<CegidSqlDB>()
                .AddSingleton<GttSqlDB>()
                .AddSingleton<ISaveSnapshotGateway,SqlSaveSnapshotGateway>();
        }
    }
}
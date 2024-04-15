using GT.Trace.Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GT.Trace.ConfigurationPanel.Infra.DataSources;
using GT.Trace.ConfigurationPanel.Infra.Repositories;
using GT.Trace.ConfigurationPanel.Domain.Repositories;

namespace GT.Trace.ConfigurationPanel.Infra
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddLoggingServices(configuration) //GT.Trace.Common.Logging se agrega esta libreria para el ensamblador.
                .AddSingleton(typeof(ConfigurationSqlDbConnectionFactory<>))
                .AddSingleton(typeof(ConfigurationSqlDbConnection<>))
                .AddSingleton<AppsSqlDB>()
                .AddSingleton<TrazaSqlDB>()
                .AddSingleton<CegidSqlDB>()
                .AddSingleton<GttSqlDB>()
                .AddSingleton<IBomRepository,BomRepository>()
                .AddSingleton<ILineRepository,LineRepository>()
                .AddSingleton<IMasterRepository,MasterRepository>()
                .AddSingleton<IPatentRepository,PatentRepository>()
                .AddSingleton<IPointOfUseEtisRepository,PointOfUseEtisRepository>();
        }
    }
}

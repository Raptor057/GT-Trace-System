using GT.Trace.Common.Infra.DataSources.SqlDB;
using GT.Trace.Common.Infra.DataSources.SqlDB.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.Common.Infra
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddCommonInfraServices(this IServiceCollection services)
        {
            return services
                .AddSingleton(typeof(GenericConfigurationSqlDbConnectionFactory<>))
                .AddSingleton(typeof(IGenericDB<>), typeof(GenericDB<>))
                .AddSingleton<IGtt, GttDB>()
                .AddSingleton<ITraza, TrazaDB>()
                .AddSingleton<IApps, AppsDB>()
                .AddSingleton<IPmi, PmiDB>();
        }
    }
}
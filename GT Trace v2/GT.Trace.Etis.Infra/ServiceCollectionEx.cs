using GT.Trace.Common.Infra;
using GT.Trace.Common.Logging;
using GT.Trace.Etis.Domain.Repositories;
using GT.Trace.Etis.Domain.Services;
using GT.Trace.Etis.Infra.Daos;
using GT.Trace.Etis.Infra.Repositories;
using GT.Trace.Etis.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.Etis.Infra
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddLoggingServices(configuration)
                .AddSingleton<IAppsSqlDBConnection, AppsSqlDBConnection>()
                .AddSingleton<ITrazaSqlDBConnection, TrazaSqlDBConnection>()
                .AddSingleton<IGttSqlDBConnection, GttSqlDBConnection>()
                .AddSingleton<AppsConfigurableSqlDatabaseConnectionFactory>()
                .AddSingleton<GttConfigurableSqlDatabaseConnectionFactory>()
                .AddSingleton<TrazaConfigurableSqlDatabaseConnectionFactory>()
                .AddTransient<SubEtiDao>()
                .AddTransient<EtiDao>()
                .AddTransient<MotorsEtiDao>()
                .AddTransient<IEtiParserService, ConfigurableRegExEtiParserService>()
                .AddTransient<IEtiRepository, SqlEtiRepository>();
        }
    }
}
using GT.Trace.Common.Infra;
using GT.Trace.Common.Logging;
using GT.Trace.EtiMovements.App.Services;
using GT.Trace.EtiMovements.Domain.Repositories;
using GT.Trace.EtiMovements.Infra.Daos;
using GT.Trace.EtiMovements.Infra.Repositories;
using GT.Trace.EtiMovements.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.EtiMovements.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddLoggingServices(configuration)
                .AddTransient<ILineServices, HttpLineServices>()
                .AddTransient<IEtiServices, HttpEtiServices>()
                .AddTransient<IEtiRepository, SqlEtiRepository>()
                .AddTransient<ILineRepository, SqlLineRepository>()
                .AddTransient<PointOfUseEtiDao>()
                //.AddTransient<UpdateEtiTrazaDao>() //Se agrego este Dao para ser usado en la implementacion de UpdateEtiTraza
                .AddSingleton<IAppsSqlDBConnection, AppsSqlDBConnection>()
                .AddSingleton<ITrazaSqlDBConnection, TrazaSqlDBConnection>()
                .AddSingleton<IGttSqlDBConnection, GttSqlDBConnection>()
                .AddSingleton<AppsConfigurableSqlDatabaseConnectionFactory>()
                .AddSingleton<GttConfigurableSqlDatabaseConnectionFactory>()
                .AddSingleton<TrazaConfigurableSqlDatabaseConnectionFactory>();
        }
    }
}
using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis;
using GT.Trace.Common.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.WagonLoad.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                //.AddLoggingServices(configuration)
                //.AddTransient<ILineServices, HttpLineServices>()
                //.AddTransient<IEtiServices, HttpEtiServices>()
                //.AddTransient<IEtiRepository, SqlEtiRepository>()
                //.AddTransient<ILineRepository, SqlLineRepository>()
                //.AddTransient<PointOfUseEtiDao>()
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

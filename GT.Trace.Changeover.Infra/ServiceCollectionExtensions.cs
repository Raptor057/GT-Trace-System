using GT.Trace.Changeover.App.Gateways;
using GT.Trace.Changeover.App.UseCases.ApplyChangeover;
using GT.Trace.Changeover.Infra.Daos;
using GT.Trace.Changeover.Infra.Gateways;
using GT.Trace.Changeover.Infra.Services;
using GT.Trace.Common.Infra;
using GT.Trace.Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.Changeover.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddLoggingServices(configuration)
                .AddTransient<WorkOrderDao>()
                .AddTransient<LineDao>()
                .AddTransient<GammaDao>()
                .AddTransient<PointOfUseDao>()
                .AddTransient<ProductionScheduleDao>()
                .AddTransient<IReturnLabelPrintingService, HttpReturnLabelPrintingService>()
                .AddTransient<IPointOfUseGateway, SqlPointOfUseGateway>()
                .AddTransient<IProductionScheduleGateway, SqlProductionScheduleGateway>()
                .AddTransient<IWorkOrderGateway, SqlWorkOrderGateway>()
                .AddTransient<ILineGateway, SqlLineGateway>()
                .AddTransient<IGammaGateway, SqlGammaGateway>()
                .AddSingleton<IAppsSqlDBConnection, AppsSqlDBConnection>()
                .AddSingleton<ITrazaSqlDBConnection, TrazaSqlDBConnection>()
                .AddSingleton<IGttSqlDBConnection, GttSqlDBConnection>()
                .AddSingleton<AppsConfigurableSqlDatabaseConnectionFactory>()
                .AddSingleton<GttConfigurableSqlDatabaseConnectionFactory>()
                .AddSingleton<TrazaConfigurableSqlDatabaseConnectionFactory>();
        }
    }
}
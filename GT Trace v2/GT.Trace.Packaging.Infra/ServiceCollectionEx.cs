using GT.Trace.Common.Logging;
using GT.Trace.Packaging.App.Gateways;
using GT.Trace.Packaging.App.Services;
using GT.Trace.Packaging.App.UseCases.Auth;
using GT.Trace.Packaging.App.UseCases.SetLineHeadcount;
using GT.Trace.Packaging.App.UseCases.LoadLines;
using GT.Trace.Packaging.Domain.Repositories;
using GT.Trace.Packaging.Infra.Daos.Auth;
using GT.Trace.Packaging.Infra.Daos.LoadLines;
using GT.Trace.Packaging.Infra.DataSources;
using GT.Trace.Packaging.Infra.Gateways;
using GT.Trace.Packaging.Infra.Repositories;
using GT.Trace.Packaging.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GT.Trace.Packaging.App.UseCases.SetStationBlocked;

namespace GT.Trace.Packaging.Infra
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
                .AddSingleton<CegidSqlDB>()
                .AddSingleton<GttSqlDB>()
                .AddSingleton<ILineHeadcountGateway, SqlLineHeadcountGateway>()
                .AddSingleton<IHourlyProductionGateway, SqlHourlyProductionGateway>()
                .AddSingleton<IMasterLabelsGateway, SqlMasterLabelsGateway>()
                .AddSingleton<IPrintingService, PrintingService>()
                .AddSingleton<ILabelParserService, LabelParserService>()
                .AddSingleton<INotificationsService, NotificationsService>()
                .AddSingleton<IStationRepository, StationRepository>()
                .AddSingleton<IUnitRepository, UnitRepository>()
                //.AddSingleton<IStationsDao, StationsSqlDao>()
                .AddSingleton<ISetStationBlockedGateway,SqlSetStationBlockedRepository>()
                .AddSingleton<ILinesDao, LinesSqlDao>()
                .AddSingleton<IUsersDao, UsersSqlDao>();
        }
    }
}
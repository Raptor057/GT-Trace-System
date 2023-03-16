using GT.Trace.Common.Logging;
using GT.Trace.EZ2000.Packaging.App.Gateways;
using GT.Trace.EZ2000.Packaging.App.Services;
using GT.Trace.EZ2000.Packaging.App.UseCases.Auth;
using GT.Trace.EZ2000.Packaging.App.UseCases.SetLineHeadcount;
using GT.Trace.EZ2000.Packaging.App.UseCases.LoadLines;
using GT.Trace.EZ2000.Packaging.Domain.Repositories;
using GT.Trace.EZ2000.Packaging.Infra.Daos.Auth;
using GT.Trace.EZ2000.Packaging.Infra.Daos.LoadLines;
using GT.Trace.EZ2000.Packaging.Infra.DataSources;
using GT.Trace.EZ2000.Packaging.Infra.Gateways;
using GT.Trace.EZ2000.Packaging.Infra.Repositories;
using GT.Trace.EZ2000.Packaging.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GT.Trace.EZ2000.Packaging.App.UseCases.SetStationBlocked;
using GT.Trace.EZ2000.Packaging.App.UseCases.UpdateActiveEti;

namespace GT.Trace.EZ2000.Packaging.Infra
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
                .AddSingleton<IUpdateActiveEti, SqlUpdateActiveEtiGateway>()
                .AddSingleton<IUnitRepository, UnitRepository>()
                .AddSingleton<ISetStationBlockedGateway,SqlSetStationBlockedRepository>()
                .AddSingleton<ILinesDao, LinesSqlDao>()
                .AddSingleton<IUsersDao, UsersSqlDao>();
        }
    }
}
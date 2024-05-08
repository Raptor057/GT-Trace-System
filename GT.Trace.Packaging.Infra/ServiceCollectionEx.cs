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
using GT.Trace.Packaging.App.UseCases.JoinFramelessMotors;
using GT.Trace.Packaging.App.UseCases.JoinEZMotors;
using GT.Trace.Packaging.App.UseCases.JoinLinePallet;
using GT.Trace.Packaging.App.UseCases.SaveEzMotors;
using GT.Trace.Packaging.App.UseCases.UnpackUnit;

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
                .AddSingleton<IJoinFramelessMotorsGateway, SqlJoinFramelessMotorsGateway>()//Se agrega esto para hacer Join de los motores en LP  RA: 06/22/2023
                .AddSingleton<IJoinEZMotorsGateway,SqlJoinEZMotorsGateway>()//Se agrega esto para hacer Join de los motores en LP  RA: 06/27/2023
                .AddSingleton<IJoinLinePalletGateway,SqlJoinPalletGateway>()// Se agrego esto para hacer Join entre las transmisiones y los pallet RA: 09/12/2023
                .AddSingleton<ISaveEzMotorsGateway,SqlSaveEzMotorsGateway>()//Se agrego esto para registrar los motores de EZ en la linea E1 RA: 11/22/2023
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
                .AddSingleton<IUsersDao, UsersSqlDao>()
                .AddSingleton< IUnpackUnitGateway,SqlUnpackUnitGateway>();
        }
    }
}
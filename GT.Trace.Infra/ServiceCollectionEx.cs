using GT.Trace.App.Services;
using GT.Trace.App.UseCases.Lines.GetBom;
using GT.Trace.App.UseCases.Lines.GetCurrentHourProduction;
using GT.Trace.App.UseCases.Lines.GetHourlyProduction;
using GT.Trace.App.UseCases.Lines.GetLine;
using GT.Trace.App.UseCases.Lines.GetLoadState;
using GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID;
using GT.Trace.App.UseCases.Lines.GetPointsOfUse;
using GT.Trace.App.UseCases.Lines.GetWorkOrder;
using GT.Trace.App.UseCases.Lines.UpdateGama;
using GT.Trace.App.UseCases.MaterialLoading.FetchLines;
using GT.Trace.App.UseCases.MaterialLoading.FetchLineWorkOrders;
using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis;
using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseLines;
using GT.Trace.Common.Infra;
using GT.Trace.Common.Logging;
using GT.Trace.Domain.PointsOfUse.Repositories;
using GT.Trace.Domain.Repositories;
using GT.Trace.Domain.Services;
using GT.Trace.Infra.Daos;
using GT.Trace.Infra.Gateways;
using GT.Trace.Infra.Repositories;
using GT.Trace.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.Infra
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddLoggingServices(configuration)
                .AddTransient<IPointOfUseService, PointOfUseWebService>()
                .AddTransient<IBomService, SqlBomService>()
                .AddTransient<ICegidRadioService, CegidRadioWebService>()
                .AddTransient<ILabelParserService, ConfigurableRegExLabelParserService>()
                .AddTransient<IEtiService, EtiWebApiService>()
                .AddSingleton<IAppsSqlDBConnection, AppsSqlDBConnection>()
                .AddSingleton<ITrazaSqlDBConnection, TrazaSqlDBConnection>()
                .AddSingleton<IGttSqlDBConnection, GttSqlDBConnection>()
                .AddSingleton<AppsConfigurableSqlDatabaseConnectionFactory>()
                .AddSingleton<GttConfigurableSqlDatabaseConnectionFactory>()
                .AddSingleton<TrazaConfigurableSqlDatabaseConnectionFactory>()
                .AddTransient<BomDao>()
                .AddTransient<EtiDao>()
                .AddTransient<EventLogDao>()
                .AddTransient<LineDao>()
                .AddTransient<PointOfUseDao>()
                .AddTransient<StationDao>()
                .AddTransient<SubEtiDao>()
                .AddTransient<WorkOrderDao>()
                .AddTransient<SubAssemblyDao>()
                .AddTransient<ProductionDao>()
                .AddTransient<LinePointsOfUseDao>()
                .AddTransient<GttDao>()
                .AddTransient<ILineRepository, SqlLineRepository>()
                .AddTransient<IEtiRepository, SqlEtiRepository>()
                .AddTransient<IBomRepository, SqlBomRepository>()
                .AddTransient<IPointOfUseEtiRepository, SqlPointOfUseEtiRepository>()
                .AddTransient<IFetchLinesRepository, SqlFetchLinesRepository>()
                .AddTransient<IFetchPointOfUseEtisRepository, SqlFetchPointOfUseEtisRepository>()
                .AddTransient<IFetchLineWorkOrdersRepository, SqlFetchLineWorkOrdersRepository>()
                .AddTransient<IFetchPointOfUseLinesRepository, SqlFetchPointOfUseLinesRepository>()
                .AddTransient<IGetLineRepository, SqlGetLineRepository>()
                .AddTransient<IGetNewSubAssemblyIDRepository, SqlGetNewSubAssemblyIDRepository>()
                .AddTransient<IWorkOrderGateway, SqlWorkOrderGateway>()
                .AddTransient<IGetHourlyProductionGateway, SqlGetLineHourlyProductionGateway>()
                .AddTransient<IGetCurrentHourProductionGateway, SqlGetLineCurrentHourProductionGateway>()
                .AddTransient<IGetPointsOfUseGateway, SqlGetLinePointsOfUseGateway>()
                .AddTransient<IGetWorkOrderGateway, SqlGetLineWorkOrderGateway>()
                .AddTransient<IGetBomGateway, SqlGetLineBomGateway>()
                .AddTransient<IUpdateBomLineGateway,SqlUpdateGamaGateway>()// Agregadas el 08/30/2023 para el endpoint de actualizacion de gama
                .AddTransient<ILoadStateGateway, SqlLoadStateGateway>();
        }
    }
}
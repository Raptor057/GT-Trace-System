using GT.Trace.App.UseCases.Lines.GetCurrentHourProduction;
using GT.Trace.Infra.Daos;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Infra.Gateways
{
    internal class SqlGetLineCurrentHourProductionGateway : IGetCurrentHourProductionGateway
    {
        private readonly ProductionDao _production;

        private readonly ILogger<SqlGetLineHourlyProductionGateway> _logger;

        public SqlGetLineCurrentHourProductionGateway(ProductionDao production, ILogger<SqlGetLineHourlyProductionGateway> logger)
        {
            _production = production;
            _logger = logger;
        }

        public async Task<ProductionDto?> GetProductionByLineAsync(string lineCode)
        {
            var item = await _production.GetLineCurrentHourProductionAsync(lineCode).ConfigureAwait(false);
            if (item == null)
            {
                return null;
            }
            return new ProductionDto(item.LineCode, item.Interval, item.ActualQuantity, item.ExpectedQuantity, item.Forecast, item.ExpectedRate, item.ActualRate, item.Requirement);
        }
    }
}
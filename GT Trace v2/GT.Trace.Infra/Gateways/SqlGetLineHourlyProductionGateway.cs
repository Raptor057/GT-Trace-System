using GT.Trace.App.UseCases.Lines.GetHourlyProduction;
using GT.Trace.Infra.Daos;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Infra.Gateways
{
    internal class SqlGetLineHourlyProductionGateway : IGetHourlyProductionGateway
    {
        private readonly ProductionDao _production;

        private readonly ILogger<SqlGetLineHourlyProductionGateway> _logger;

        public SqlGetLineHourlyProductionGateway(ProductionDao production, ILogger<SqlGetLineHourlyProductionGateway> logger)
        {
            _production = production;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductionDto>> GetProductionByLineAsync(string lineCode, DateTime? workDayDate = null)
        {
            var production = await _production.GetLineHourlyProductionByWorkDayAsync(lineCode, workDayDate).ConfigureAwait(false);
            return production.Select(item => new ProductionDto(item.IntervalName, item.HourlyRequirement, item.PartNo, item.Quantity));
        }
    }
}
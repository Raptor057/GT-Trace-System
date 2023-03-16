using GT.Trace.EZ2000.Packaging.App.Dtos;
using GT.Trace.EZ2000.Packaging.App.Gateways;
using GT.Trace.EZ2000.Packaging.Infra.DataSources;

namespace GT.Trace.EZ2000.Packaging.Infra.Gateways
{
    internal sealed class SqlHourlyProductionGateway : IHourlyProductionGateway
    {
        private readonly GttSqlDB _gtt;

        private readonly AppsSqlDB _apps;

        public SqlHourlyProductionGateway(GttSqlDB gtt, AppsSqlDB apps)
        {
            _gtt = gtt;
            _apps = apps;
        }

        public async Task<IEnumerable<HourlyProductionItemDto>> GetHourlyProductionByLineAsync(string lineCode)
        {
            var line = await _apps.GetLineByCodeAsync(lineCode).ConfigureAwait(false);
            if (line == null)
            {
                return Enumerable.Empty<HourlyProductionItemDto>();
            }
            var intervalProduction = await _gtt.GetTimeIntervalProduction(lineCode, line.Prod_Hour ?? 0).ConfigureAwait(false);
            return intervalProduction.Select(prod =>
                new HourlyProductionItemDto(
                    prod.Name,
                    prod.IsPastDue,
                    prod.IsCurrent,
                    prod.PartNo,
                    prod.EffectiveHourlyRequirement,
                    prod.Quantity,
                    prod.Pph,
                    prod.Headcount));
        }
    }
}
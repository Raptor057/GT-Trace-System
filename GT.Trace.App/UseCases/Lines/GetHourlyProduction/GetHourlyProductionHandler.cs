using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.App.UseCases.Lines.GetHourlyProduction
{
    internal class GetHourlyProductionHandler : ResultInteractorBase<GetHourlyProductionRequest, GetHourlyProductionResponse>
    {
        private readonly ILogger<GetHourlyProductionHandler> _logger;

        private readonly IGetHourlyProductionGateway _gateway;

        public GetHourlyProductionHandler(ILogger<GetHourlyProductionHandler> logger, IGetHourlyProductionGateway gateway)
        {
            _logger = logger;
            _gateway = gateway;
        }

        public override async Task<Result<GetHourlyProductionResponse>> Handle(GetHourlyProductionRequest request, CancellationToken cancellationToken)
        {
            var production = await _gateway.GetProductionByLineAsync(request.LineCode, null).ConfigureAwait(false);

            var intervals = production.Select(p => p.Interval).Distinct();
            var partNumbers = production.Where(p => !string.IsNullOrWhiteSpace(p.PartNo)).Select(p => p.PartNo).Distinct();
            return OK(new GetHourlyProductionResponse(
                intervals,
                partNumbers.ToDictionary(pn => pn!, pn => intervals.ToDictionary(interval => interval!, interval => production.SingleOrDefault(p => p.Interval == interval && p.PartNo == pn)!))
            ));
        }
    }
}
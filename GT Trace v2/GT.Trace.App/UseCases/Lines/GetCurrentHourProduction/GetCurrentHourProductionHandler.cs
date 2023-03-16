using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.App.UseCases.Lines.GetCurrentHourProduction
{
    internal class GetCurrentHourProductionHandler : ResultInteractorBase<GetCurrentHourProductionRequest, GetCurrentHourProductionResponse>
    {
        private readonly ILogger<GetCurrentHourProductionHandler> _logger;

        private readonly IGetCurrentHourProductionGateway _gateway;

        public GetCurrentHourProductionHandler(ILogger<GetCurrentHourProductionHandler> logger, IGetCurrentHourProductionGateway gateway)
        {
            _logger = logger;
            _gateway = gateway;
        }

        public override async Task<Result<GetCurrentHourProductionResponse>> Handle(GetCurrentHourProductionRequest request, CancellationToken cancellationToken)
        {
            var production = await _gateway.GetProductionByLineAsync(request.LineCode).ConfigureAwait(false);
            if (production == null)
            {
                return Fail($"No se encontró información sobre la línea \"{request.LineCode}\".");
            }
            return OK(new GetCurrentHourProductionResponse(production));
        }
    }
}
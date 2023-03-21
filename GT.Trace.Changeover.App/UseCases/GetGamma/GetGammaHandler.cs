using GT.Trace.Changeover.App.Gateways;
using GT.Trace.Changeover.App.UseCases.GetGamma;
using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Changeover.App.Dtos
{
    internal sealed class GetGammaHandler : IInteractor<GetGammaRequest, GetGammaResponse>
    {
        private readonly ILogger<GetGammaHandler> _logger;

        private readonly IGammaGateway _gamma;

        public GetGammaHandler(ILogger<GetGammaHandler> logger, IGammaGateway gamma)
        {
            _logger = logger;
            _gamma = gamma;
        }

        public async Task<GetGammaResponse> Handle(GetGammaRequest request, CancellationToken cancellationToken)
        {
            var gamma = await _gamma.GetGammaAsync(request.LineCode, request.PartNo, request.Revision).ConfigureAwait(false);
            return new GetGammaSuccessResponse(gamma);
        }
    }
}
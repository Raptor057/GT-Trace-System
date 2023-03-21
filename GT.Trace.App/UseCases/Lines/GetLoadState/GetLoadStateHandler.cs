using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.App.UseCases.Lines.GetLoadState
{
    internal sealed class GetLoadStateHandler : IInteractor<GetLoadStateRequest, GetLoadStateResponse>
    {
        private readonly ILogger<GetLoadStateHandler> _logger;

        private readonly ILoadStateGateway _gateway;

        public GetLoadStateHandler(ILogger<GetLoadStateHandler> logger, ILoadStateGateway gateway)
        {
            _logger = logger;
            _gateway = gateway;
        }

        public async Task<GetLoadStateResponse> Handle(GetLoadStateRequest request, CancellationToken cancellationToken)
        {
            var etis = await _gateway.GetLineLoadedMaterialAsync(request.LineCode).ConfigureAwait(false);
            var gama = await _gateway.GetGamaAsync(request.PartNo, request.LineCode).ConfigureAwait(false);
            return new GetLoadStateSuccessResponse(gama.ToDictionary(item => item, item => etis.Where(eti => string.Compare(eti.ComponentNo, item.ComponentNo, true) == 0 && string.Compare(eti.PointOfUseCode, item.PointOfUseCode, true) == 0)));
        }
    }
}
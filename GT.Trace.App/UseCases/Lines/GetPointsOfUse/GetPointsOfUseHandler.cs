using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.App.UseCases.Lines.GetPointsOfUse
{
    internal sealed class GetPointsOfUseHandler : ResultInteractorBase<GetPointsOfUseRequest, GetPointsOfUseResponse>
    {
        private readonly ILogger<GetPointsOfUseHandler> _logger;

        private readonly IGetPointsOfUseGateway _gateway;

        public GetPointsOfUseHandler(ILogger<GetPointsOfUseHandler> logger, IGetPointsOfUseGateway gateway)
        {
            _logger = logger;
            _gateway = gateway;
        }

        public override async Task<Result<GetPointsOfUseResponse>> Handle(GetPointsOfUseRequest request, CancellationToken cancellationToken)
        {
            var pointOfUseCodes = await _gateway.GetEnabledPointsOfUseAsync(request.LineCode).ConfigureAwait(false);
            return OK(new GetPointsOfUseResponse(pointOfUseCodes));
        }
    }
}
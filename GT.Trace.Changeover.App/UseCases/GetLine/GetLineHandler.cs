using GT.Trace.Changeover.App.Gateways;
using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Changeover.App.UseCases.GetLine
{
    internal sealed class GetLineHandler : IInteractor<GetLineByCodeRequest, GetLineResponse>
    {
        private readonly ILogger<GetLineHandler> _logger;

        private readonly ILineGateway _lines;

        public GetLineHandler(ILogger<GetLineHandler> logger, ILineGateway lines)
        {
            _logger = logger;
            _lines = lines;
        }

        public async Task<GetLineResponse> Handle(GetLineByCodeRequest request, CancellationToken cancellationToken)
        {
            var line = await _lines.GetLineAsync(request.LineCode).ConfigureAwait(false);
            if (line == null)
            {
                return new GetLineFailureResponse($"No se encontró la línea \"{request.LineCode}\".");
            }

            return new GetLineSuccessResponse(line);
        }
    }
}
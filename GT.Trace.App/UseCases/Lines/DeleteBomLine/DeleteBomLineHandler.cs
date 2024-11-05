using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.App.UseCases.Lines.DeleteBomLine
{
    internal sealed class DeleteBomLineHandler : IInteractor<DeleteBomLineRequest,DeleteBomLineResponse>
    {
        private readonly ILogger<DeleteBomLineHandler> _logger;
        private readonly IDeleteBomLineGateway _gateway;

        public DeleteBomLineHandler(ILogger<DeleteBomLineHandler> logger, IDeleteBomLineGateway gateway)
        {
            _logger=logger;
            _gateway=gateway;
        }

        public async Task<DeleteBomLineResponse> Handle(DeleteBomLineRequest request, CancellationToken cancellationToken)
        {
            await _gateway.DeleteGamaTrazabAsync(request.ogpartNo,request.oglineCode);
            _logger.LogInformation($"Gama {request.ogpartNo} {request.oglineCode} Borrada en TRAZAB");

            await _gateway.DeleteGamaTrazabAsync(request.icpartNo, request.iclineCode);
            
            _logger.LogInformation($"Gama {request.icpartNo} {request.iclineCode} Borrada en TRAZAB");

            //return new DeleteBomLineSuccessResponse($"Gamas Borradas {request.ogpartNo} {request.oglineCode} & {request.icpartNo} {request.iclineCode} en TRAZAB");
            return new DeleteBomLineSuccessResponse();
        }
    }
}

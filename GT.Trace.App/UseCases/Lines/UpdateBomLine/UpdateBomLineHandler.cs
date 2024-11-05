using GT.Trace.App.UseCases.Lines.UpdateBomLine;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.App.UseCases.Lines.UpdateGama
{
    internal sealed class UpdateBomLineHandler :IInteractor<UpdateBomLineRequest, UpdateBomLineResponse>
    {
        private readonly ILogger<UpdateBomLineHandler> _logger;
        private readonly IUpdateBomLineGateway _updateBomLine;

        public UpdateBomLineHandler(ILogger<UpdateBomLineHandler> logger, IUpdateBomLineGateway updateBomLine)
        {
            _logger=logger;
            _updateBomLine=updateBomLine;
        }

        public async Task<UpdateBomLineResponse> Handle(UpdateBomLineRequest request, CancellationToken cancellationToken)
        {
            //_ = await _updateBomLine.UpdateGamaTrazab(request.PartNo, request.LineCode).ConfigureAwait(false);
            //_ = await _updateBomLine.UpdateGamaGtt(request.PartNo, request.LineCode).ConfigureAwait(false);


            _ = await _updateBomLine.UpdateGamaTrazab(request.ogpartNo, request.oglineCode).ConfigureAwait(false);
            _logger.LogInformation($"Se actualizo la gama {request.ogpartNo} {request.oglineCode} en TRAZAB");
            _ = await _updateBomLine.UpdateGamaGtt(request.ogpartNo, request.oglineCode).ConfigureAwait(false);
            _logger.LogInformation($"Se actualizo la gama {request.ogpartNo} {request.oglineCode} en GTT");

            _ = await _updateBomLine.UpdateGamaTrazab(request.icpartNo, request.iclineCode).ConfigureAwait(false);
            _logger.LogInformation($"Se actualizo la gama {request.icpartNo} {request.iclineCode} en TRAZAB");
            _ = await _updateBomLine.UpdateGamaGtt(request.icpartNo, request.iclineCode).ConfigureAwait(false);
            _logger.LogInformation($"Se actualizo la gama {request.icpartNo} {request.iclineCode} en GTT");

            _logger.LogInformation($"Se actualizo las gamas en ambas bases de datos TRAZAB y GTT");
            return new UpdateBomLineSuccessResponse("Gama Actualizada");
        }
    }
}

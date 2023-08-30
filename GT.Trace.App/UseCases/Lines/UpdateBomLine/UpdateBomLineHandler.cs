using GT.Trace.App.UseCases.Lines.UpdateBomLine;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.App.UseCases.Lines.UpdateGama
{
    internal sealed class UpdateBomLineHandler :IInteractor<UpdateBomLineRequest, UpdateBomLineResponse>
    {
        private readonly IUpdateBomLineGateway _updateBomLine;

        public UpdateBomLineHandler(IUpdateBomLineGateway updateBomLine)
        {
            _updateBomLine=updateBomLine;
        }

        public async Task<UpdateBomLineResponse> Handle(UpdateBomLineRequest request, CancellationToken cancellationToken)
        {
            _ = await _updateBomLine.UpdateGama(request.PartNo, request.LineCode).ConfigureAwait(false);
            return new UpdateBomLineSuccessResponse("Gama Actualizada");
        }
    }
}

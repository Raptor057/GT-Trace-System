using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.UpdateActiveEti
{
    internal class UpdateEtiTrazaHandler : IInteractor<UpdateEtiTrazaRequest, UpdateEtiTrazaResponse>
    {
        private readonly ILogger<UpdateEtiTrazaHandler> _logger;
        private readonly IUpdateActiveEti _gateway;

        public UpdateEtiTrazaHandler(ILogger<UpdateEtiTrazaHandler> logger, IUpdateActiveEti gateway)
        {
            _logger=logger;
            _gateway=gateway;
        }

        public async Task<UpdateEtiTrazaResponse> Handle(UpdateEtiTrazaRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _gateway.UpdateActiveEtiAsync(request.etiNo).ConfigureAwait(false);
                return new UpdateEtiTrazaSuccess();
            }
            catch (Exception ex)
            {
                return new UpdateEtiTrazaFailure(ex.Message);
            }
        }
    }
}

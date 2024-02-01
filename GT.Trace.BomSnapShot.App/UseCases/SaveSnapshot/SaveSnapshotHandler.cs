using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot
{
    public sealed class SaveSnapshotHandler : IInteractor<SaveSnapshotRequest, SaveSnapshotResponse>
    {
        private readonly ILogger<SaveSnapshotHandler> _logger;
        private readonly ISaveSnapshotGateway _gateway;

        public SaveSnapshotHandler(ILogger<SaveSnapshotHandler> logger, ISaveSnapshotGateway gateway)
        {
            _logger=logger;
            _gateway=gateway;
        }
        public async Task<SaveSnapshotResponse> Handle(SaveSnapshotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var SaveSnapshotMessage =  await _gateway.SaveSnapshotAsync(request.EtiNo, request.LineCode).ConfigureAwait(false);
                return new SaveSnapshotSuccess(SaveSnapshotMessage);
            }
            catch (Exception ex)
            {
                return new SaveSnapshotFailure(ex.Message);
            }
        }
    }
}

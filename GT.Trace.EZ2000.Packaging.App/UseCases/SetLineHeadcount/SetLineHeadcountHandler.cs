using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.SetLineHeadcount
{
    internal sealed class SetLineHeadcountHandler : IInteractor<SetLineHeadcountRequest, SetLineHeadcountResponse>
    {
        private readonly ILogger<SetLineHeadcountHandler> _logger;

        private readonly ILineHeadcountGateway _gateway;

        public SetLineHeadcountHandler(ILogger<SetLineHeadcountHandler> logger, ILineHeadcountGateway gateway)
        {
            _logger = logger;
            _gateway = gateway;
        }

        public async Task<SetLineHeadcountResponse> Handle(SetLineHeadcountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _gateway.SetCurrentShiftWorkOrderHeadcountAsync(request.LineCode, request.WorkOrderCode, request.Headcount).ConfigureAwait(false);
                return new SetLineHeadcountSuccess();
            }
            catch (Exception ex)
            {
                return new SetLineHeadcountFailure(ex.Message);
            }
        }
    }
}
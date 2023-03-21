using GT.Trace.Common.CleanArch;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.SetStationBlocked
{
    internal class SetStationBlockedHandler : IInteractor<SetStationBlockedRequest, SetStationBlockedResponse>
    {
        private readonly ISetStationBlockedGateway _gateway;

        public SetStationBlockedHandler(ISetStationBlockedGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<SetStationBlockedResponse> Handle(SetStationBlockedRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _gateway.StationBlocked(request.Is_blocked, request.LineName).ConfigureAwait(false);
                return new SetStationBlockedResponse();
            }
            catch (Exception ex)
            {
                return new ISetStationBlockedFailure(ex.Message);
            }
                
        }
    }
}

using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.Gateways;

namespace GT.Trace.Packaging.App.UseCases.GetLastMasterFolioByLine
{
    internal sealed class GetLastMasterFolioByLineHandler : IInteractor<GetLastMasterFolioByLineRequest, GetLastMasterFolioByLineResponse>
    {
        private readonly IMasterLabelsGateway _gateway;

        public GetLastMasterFolioByLineHandler(IMasterLabelsGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<GetLastMasterFolioByLineResponse> Handle(GetLastMasterFolioByLineRequest request, CancellationToken cancellationToken)
        {
            var folio = await _gateway.GetLastMasterFolioByLineAsync(request.LineName).ConfigureAwait(false);
            return new GetLastMasterFolioByLineResponse(folio, request.LineCode, request.LineName);
        }
    }
}
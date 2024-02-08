using GT.Trace.BomSnapShot.App.Gateways;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.BomSnapShot.App.UseCases.GetSeqSnapshotID
{
    public record GetSeqSnapshotIDResponse(long SeqSnapshots) : IResponse;
    public sealed record GetSeqSnapshotIDRequest(string lineCode, string partNo): IRequest<GetSeqSnapshotIDResponse>;
    internal sealed class GetSeqSnapshotIDHandler : IInteractor<GetSeqSnapshotIDRequest, GetSeqSnapshotIDResponse>
    {
        private readonly IGetSeqSnapshotIDGateways _gateway;

        public GetSeqSnapshotIDHandler(IGetSeqSnapshotIDGateways gateway)
        {
            _gateway=gateway;
        }
        public async Task<GetSeqSnapshotIDResponse> Handle(GetSeqSnapshotIDRequest request, CancellationToken cancellationToken)
        {
            return new GetSeqSnapshotIDResponse(await _gateway.GetSeqSnapshotIDByLineCodeandPartNo(request.lineCode, request.partNo).ConfigureAwait(false));
        }
    }
}

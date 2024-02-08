using GT.Trace.BomSnapShot.App.UseCases.GetSeqSnapshotID;
using GT.Trace.BomSnapShotWebApi.Endpoints.SeqSnapshotID;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.BomSnapShotWebApi.Endpoints.GetSeqSnapshotID
{
    public sealed class GetSeqSnapshotIDPresenter<T> : IPresenter<GetSeqSnapshotIDResponse>
        where T : GetSeqSnapshotIDResponse
    {
        private readonly GenericViewModel<GetSeqSnapshotIDController> _viewModel;

        public GetSeqSnapshotIDPresenter(GenericViewModel<GetSeqSnapshotIDController> viewModel)
        {
            _viewModel=viewModel;
        }
        public Task Handle(GetSeqSnapshotIDResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.OK(notification);
            return Task.CompletedTask;
        }
    }
}

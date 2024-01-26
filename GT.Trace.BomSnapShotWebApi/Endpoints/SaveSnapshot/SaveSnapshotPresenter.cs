using GT.Trace.Common.CleanArch;
using GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot;
using GT.Trace.BomSnapShotWebApi.Endpoints.SaveSnapshot;

namespace GT.Trace.BomSnapShotWebApi.Endpoints.TestSnapshot
{
    public sealed class SaveSnapshotPresenter<T> : IPresenter<SaveSnapshotSuccess>, IPresenter<SaveSnapshotFailure> where T : SaveSnapshotResponse
    {
        private readonly GenericViewModel<SaveSnapshotController> _viewModel;

        public SaveSnapshotPresenter(GenericViewModel<SaveSnapshotController> viewModel)
        {
            _viewModel=viewModel;
        }
        public async Task Handle(SaveSnapshotSuccess notification, CancellationToken cancellationToken)
        {
            _viewModel.OK(notification.Message ?? "");
            await Task.CompletedTask;
        }

        public Task Handle(SaveSnapshotFailure notification, CancellationToken cancellationToken)
        {
            _viewModel.Fail(notification.Message ?? "");
            return Task.CompletedTask;
        }
    }
}

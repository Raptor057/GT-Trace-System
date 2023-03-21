using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.LoadPackState.Responses;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.GetLineStatus
{
    public sealed class GetLineStatusPresenter<T> : IPresenter<FailureLoadPackStateResponse>, IPresenter<SuccessLoadPackStateResponse>
        where T : SuccessLoadPackStateResponse
    {
        private readonly GenericViewModel<GetLineStatusController> _viewModel;

        public GetLineStatusPresenter(GenericViewModel<GetLineStatusController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(FailureLoadPackStateResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.Fail(notification.Message);
            return Task.CompletedTask;
        }

        public Task Handle(SuccessLoadPackStateResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.OK(notification.State);
            return Task.CompletedTask;
        }
    }
}
using GT.Trace.App.UseCases.Lines.GetLoadState;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetLoadState
{
    public sealed class GetLoadStatePresenter<T> : IPresenter<GetLoadStateSuccessResponse>, IPresenter<GetLoadStateFailureResponse>
        where T : GetLoadStateResponse
    {
        private readonly GenericViewModel<GetLoadStateController> _viewModel;

        public GetLoadStatePresenter(GenericViewModel<GetLoadStateController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(GetLoadStateSuccessResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.OK(notification.State.Keys.Select(key => new { key.PointOfUseCode, key.ComponentNo, key.Capacity, Etis = notification.State[key] }));
            return Task.CompletedTask;
        }

        public Task Handle(GetLoadStateFailureResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.Fail(notification.Message);
            return Task.CompletedTask;
        }
    }
}
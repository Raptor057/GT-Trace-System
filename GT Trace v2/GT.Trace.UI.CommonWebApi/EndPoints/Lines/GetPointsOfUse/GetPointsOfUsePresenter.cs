using GT.Trace.App.UseCases.Lines.GetPointsOfUse;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetPointsOfUse
{
    public sealed class GetPointsOfUsePresenter<T> : IResultPresenter<GetPointsOfUseResponse>
        where T : Result<GetPointsOfUseResponse>
    {
        private readonly ResultViewModel<GetPointsOfUseController> _viewModel;

        public GetPointsOfUsePresenter(ResultViewModel<GetPointsOfUseController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<GetPointsOfUseResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<GetPointsOfUseResponse> success)
            {
                _viewModel.Set(success, (data) => data.EnabledPointsOfUse);
            }
            return Task.CompletedTask;
        }
    }
}
using GT.Trace.App.UseCases.MaterialLoading.FetchEtiPointsOfUse;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.PointsOfUse.FetchEtiPointsOfUse
{
    public sealed class FetchEtiPointsOfUsePresenter<T> : IResultPresenter<FetchEtiPointsOfUseResponse>
        where T : Result<FetchEtiPointsOfUseResponse>
    {
        private readonly ResultViewModel<FetchEtiPointsOfUseController> _viewModel;

        public FetchEtiPointsOfUsePresenter(ResultViewModel<FetchEtiPointsOfUseController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<FetchEtiPointsOfUseResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<FetchEtiPointsOfUseResponse> success)
            {
                _viewModel.Set(success, data => data.PointOfUseCodes);
            }
            return Task.CompletedTask;
        }
    }
}
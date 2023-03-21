using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.PointsOfUse.GetPointOfUseEtis
{
    public sealed class GetPointOfUseEtisPresenter<T> : IResultPresenter<FetchPointOfUseEtisResponse>
        where T : Result<FetchPointOfUseEtisResponse>
    {
        private readonly ResultViewModel<GetPointOfUseEtisController> _viewModel;

        public GetPointOfUseEtisPresenter(ResultViewModel<GetPointOfUseEtisController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<FetchPointOfUseEtisResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<FetchPointOfUseEtisResponse> success)
            {
                _viewModel.Set(success, data => data.Etis);
            }
            return Task.CompletedTask;
        }
    }
}
using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseLines;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.PointsOfUse.GetPointOfUseLines
{
    public sealed class GetPointOfUseLinesPresenter<T> : IResultPresenter<FetchPointOfUseLinesResponse>
        where T : Result<FetchPointOfUseLinesResponse>
    {
        private readonly ResultViewModel<GetPointOfUseLinesController> _viewModel;

        public GetPointOfUseLinesPresenter(ResultViewModel<GetPointOfUseLinesController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<FetchPointOfUseLinesResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<FetchPointOfUseLinesResponse> success)
            {
                _viewModel.Set(success, data => data.LineCodes);
            }
            return Task.CompletedTask;
        }
    }
}
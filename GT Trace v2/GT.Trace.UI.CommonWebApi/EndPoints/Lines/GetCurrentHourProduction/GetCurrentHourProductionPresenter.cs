using GT.Trace.App.UseCases.Lines.GetCurrentHourProduction;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetCurrentHourProduction
{
    public sealed class GetCurrentHourProductionPresenter<T> : IResultPresenter<GetCurrentHourProductionResponse>
        where T : Result<GetCurrentHourProductionResponse>
    {
        private readonly ResultViewModel<GetCurrentHourProductionEndPoint> _viewModel;

        public GetCurrentHourProductionPresenter(ResultViewModel<GetCurrentHourProductionEndPoint> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<GetCurrentHourProductionResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<GetCurrentHourProductionResponse> success)
            {
                _viewModel.Set(success);
            }
            return Task.CompletedTask;
        }
    }
}
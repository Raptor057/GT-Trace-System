using GT.Trace.App.UseCases.Lines.GetHourlyProduction;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetHourlyProduction
{
    public sealed class GetHourlyProductionPresenter<T> : IResultPresenter<GetHourlyProductionResponse>
        where T : Result<GetHourlyProductionResponse>
    {
        private readonly ResultViewModel<GetHourlyProductionController> _viewModel;

        public GetHourlyProductionPresenter(ResultViewModel<GetHourlyProductionController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<GetHourlyProductionResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<GetHourlyProductionResponse> success)
            {
                _viewModel.Set(success);
            }
            return Task.CompletedTask;
        }
    }
}
using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.GetHourlyProduction;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.GetHourlyProduction
{
    public sealed class GetHourlyProductionPresenter<T> : IPresenter<GetHourlyProductionResponse>
        where T : GetHourlyProductionResponse
    {
        private readonly GenericViewModel<GetHourlyProductionController> _viewModel;

        public GetHourlyProductionPresenter(GenericViewModel<GetHourlyProductionController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(GetHourlyProductionResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.OK(notification);
            return Task.CompletedTask;
        }
    }
}
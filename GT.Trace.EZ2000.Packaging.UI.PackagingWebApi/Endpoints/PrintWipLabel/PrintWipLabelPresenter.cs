using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.PrintWipLabel;
using GT.Trace.EZ2000.Packaging.UI.PackagingWebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.PrintWipLabel
{
    public class PrintWipLabelPresenter<T> : IPresenter<PrintWipLabelFailureResponse>, IPresenter<PrintWipLabelSuccessResponse>
        where T : PrintWipLabelResponse
    {
        private readonly GenericViewModel<PrintWipLabelEndPoint> _viewModel;

        private readonly IHubContext<EventsHub, IEventsHub> _hub;

        public PrintWipLabelPresenter(GenericViewModel<PrintWipLabelEndPoint> viewModel, IHubContext<EventsHub, IEventsHub> hub)
        {
            _viewModel = viewModel;
            _hub = hub;
        }

        public Task Handle(PrintWipLabelFailureResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.Fail(notification.Message);
            return Task.CompletedTask;
        }

        public async Task Handle(PrintWipLabelSuccessResponse notification, CancellationToken cancellationToken)
        {
            await _hub.Clients.All.PrintWip(notification.LineCode, JsonConvert.SerializeObject(notification.Pallet)).ConfigureAwait(false);
            _viewModel.OK(notification.Pallet);
        }
    }
}
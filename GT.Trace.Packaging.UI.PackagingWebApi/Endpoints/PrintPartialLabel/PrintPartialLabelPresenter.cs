using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.PrintPartialLabel.Responses;
using GT.Trace.Packaging.UI.PackagingWebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.PrintPartialLabel
{
    public sealed class PrintPartialLabelPresenter<T> : IPresenter<FailurePrintPartialLabelResponse>, IPresenter<SuccessPrintPartialLabelResponse>
        where T : PrintPartialLabelResponse
    {
        private readonly GenericViewModel<PrintPartialLabelController> _viewModel;

        private readonly IHubContext<EventsHub, IEventsHub> _hub;

        public PrintPartialLabelPresenter(GenericViewModel<PrintPartialLabelController> viewModel, IHubContext<EventsHub, IEventsHub> hub)
        {
            _viewModel = viewModel;
            _hub = hub;
        }

        public Task Handle(FailurePrintPartialLabelResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.Fail(notification.Message);
            return Task.CompletedTask;
        }

        public async Task Handle(SuccessPrintPartialLabelResponse notification, CancellationToken cancellationToken)
        {
            await _hub.Clients.All.PalletComplete(notification.LineCode, JsonConvert.SerializeObject(notification.Pallet)).ConfigureAwait(false);
            _viewModel.OK(notification.Pallet);
        }
    }
}
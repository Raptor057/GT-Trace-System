using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.UnpackUnit;
using GT.Trace.Packaging.UI.PackagingWebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.UnpackUnit
{
    public sealed class UnpackUnitPresenter<T> : IPresenter<SuccessUnpackUnitResponse>, IPresenter<FailureUnpackUnitResponse>
        where T : UnpackUnitResponse
    {
        private readonly GenericViewModel<UnpackUnitController> _viewModel;

        private readonly IHubContext<EventsHub, IEventsHub> _hub;

        public UnpackUnitPresenter(GenericViewModel<UnpackUnitController> viewModel, IHubContext<EventsHub, IEventsHub> hub)
        {
            _viewModel = viewModel;
            _hub = hub;
        }

        public async Task Handle(SuccessUnpackUnitResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.OK(notification);
            await _hub.Clients.All.UnitUnpacked(notification.LineCode, notification.UnitID).ConfigureAwait(false);
        }

        public Task Handle(FailureUnpackUnitResponse notification, CancellationToken cancellationToken)
        {
            _viewModel.Fail(notification.Message);
            return Task.CompletedTask;
        }
    }
}
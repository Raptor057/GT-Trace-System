using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.PackUnit.Responses;
using GT.Trace.Packaging.UI.PackagingWebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.PackUnit
{
    public sealed class PackUnitPresenter<T> : IPresenter<PackUnitResponse>
        where T : PackUnitResponse
    {
        private readonly GenericViewModel<PackUnitController> _viewModel;

        private readonly IHubContext<EventsHub, IEventsHub> _hub;

        public PackUnitPresenter(GenericViewModel<PackUnitController> viewModel, IHubContext<EventsHub, IEventsHub> hub)
        {
            _viewModel = viewModel;
            _hub = hub;
        }

        public async Task Handle(PackUnitResponse notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Fail(failure.Message);
            }
            else if (notification is UnitPickedResponse unitPickedResponse)
            {
                _viewModel.OK(unitPickedResponse);
                await _hub.Clients.All.UnitPicked(unitPickedResponse.LineCode, unitPickedResponse.UnitID).ConfigureAwait(false);
            }
            else if (notification is PalletCompleteResponse palletComplete)
            {
                _viewModel.OK(palletComplete);
                var jsonString = JsonConvert.SerializeObject(palletComplete.Pallet);
                await _hub.Clients.All.PalletComplete(palletComplete.LineCode, jsonString).ConfigureAwait(false);
            }
            else if (notification is ContainerCompleteResponse containerComplete)
            {
                _viewModel.OK(containerComplete);
                var jsonString = JsonConvert.SerializeObject(containerComplete.Container);
                await _hub.Clients.All.ContainerComplete(containerComplete.LineCode, jsonString).ConfigureAwait(false);
            }
            else if (notification is UnitPackedResponse unitPackedResponse)
            {
                _viewModel.OK(unitPackedResponse);
                await _hub.Clients.All.UnitPacked(unitPackedResponse.LineCode, unitPackedResponse.UnitID, unitPackedResponse.WorkOrderCode).ConfigureAwait(false);
            }
        }
    }
}
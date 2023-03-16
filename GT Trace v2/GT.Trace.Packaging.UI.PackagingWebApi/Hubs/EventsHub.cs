using Microsoft.AspNetCore.SignalR;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Hubs
{
    public class EventsHub : Hub<IEventsHub>
    {
        public async Task UnitPacked(string lineCode, long unitID, string workOrderCode) =>
            await Clients.All.UnitPacked(lineCode, unitID, workOrderCode).ConfigureAwait(false);

        public async Task UnitUnpacked(string lineCode, long unitID) =>
            await Clients.All.UnitUnpacked(lineCode, unitID).ConfigureAwait(false);

        public async Task UnitPicked(string lineCode, long unitID) =>
            await Clients.All.UnitPicked(lineCode, unitID).ConfigureAwait(false);

        public async Task PalletComplete(string lineCode, string jsonData) =>
            await Clients.All.PalletComplete(lineCode, jsonData).ConfigureAwait(false);

        public async Task ContainerComplete(string lineCode, string jsonData) =>
            await Clients.All.ContainerComplete(lineCode, jsonData).ConfigureAwait(false);

        public async Task PrintWip(string lineCode, string jsonData) =>
            await Clients.All.PrintWip(lineCode, jsonData).ConfigureAwait(false);
    }
}
using Microsoft.AspNetCore.SignalR;

namespace GT.Trace.EtiMovements.UI.WebApi.Hubs
{
    public class EtiMovementsHub : Hub<IEtiMovementsHub>
    {
        public async Task SendEtiLoadedSignalAsync(string lineCode, string etiNo, string componentNo, string pointOfUseCode) =>
            await Clients.All.EtiLoaded(lineCode, etiNo, componentNo, pointOfUseCode).ConfigureAwait(false);

        public async Task SendEtiUnloadedSignalAsync(string lineCode, string etiNo, string componentNo, string pointOfUseCode) =>
            await Clients.All.EtiUnloaded(lineCode, etiNo, componentNo, pointOfUseCode).ConfigureAwait(false);

        public async Task SendEtiUsedSignalAsync(string lineCode, string etiNo, string componentNo, string pointOfUseCode) =>
            await Clients.All.EtiUsed(lineCode, etiNo, componentNo, pointOfUseCode).ConfigureAwait(false);

        public async Task SendEtiReturnedSignalAsync(string lineCode, string etiNo, string partNo, string componentNo, string pointOfUseCode, string operatorNo, DateTime utcTimeStamp) =>
            await Clients.All.EtiReturned(lineCode, etiNo, partNo, componentNo, pointOfUseCode, operatorNo, utcTimeStamp).ConfigureAwait(false);
    }
}
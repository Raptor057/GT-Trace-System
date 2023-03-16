using Microsoft.AspNetCore.SignalR;

namespace GT.Trace.UI.MaterialLoadingWebApi.Hubs
{
    public class PointsOfUseHub : Hub<IPointsOfUseHub>
    {
        public async Task EtiCreated(string lineCode, long etiID, string componentNo, string revision, string compDescription, int quantity, DateTime utcTimeStamp) =>
            await Clients.All.EtiCreated(lineCode, etiID, componentNo, revision, compDescription, quantity, utcTimeStamp).ConfigureAwait(false);
    }
}
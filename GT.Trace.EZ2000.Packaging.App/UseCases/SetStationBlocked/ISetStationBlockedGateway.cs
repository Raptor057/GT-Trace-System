namespace GT.Trace.EZ2000.Packaging.App.UseCases.SetStationBlocked
{
    public interface ISetStationBlockedGateway
    {
        Task StationBlocked(string is_blocked, string lineName);
    }
}

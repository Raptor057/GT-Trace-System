namespace GT.Trace.EZ2000.Packaging.App.UseCases.SetStationBlocked
{
    public sealed record ISetStationBlockedFailure(string Message) : SetStationBlockedResponse;
}

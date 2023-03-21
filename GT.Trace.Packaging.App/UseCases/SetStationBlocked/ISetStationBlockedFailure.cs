namespace GT.Trace.Packaging.App.UseCases.SetStationBlocked
{
    public sealed record ISetStationBlockedFailure(string Message) : SetStationBlockedResponse;
}

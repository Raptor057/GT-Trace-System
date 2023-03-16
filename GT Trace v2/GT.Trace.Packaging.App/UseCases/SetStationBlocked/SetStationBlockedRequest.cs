using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.SetStationBlocked
{
    public sealed record SetStationBlockedRequest(string Is_blocked, string LineName) : IRequest<SetStationBlockedResponse>;
}

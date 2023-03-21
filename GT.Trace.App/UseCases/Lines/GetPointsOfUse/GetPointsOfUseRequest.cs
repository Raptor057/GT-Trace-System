using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetPointsOfUse
{
    public sealed record GetPointsOfUseRequest(string LineCode) : IResultRequest<GetPointsOfUseResponse>;
}
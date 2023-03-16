using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetLoadState
{
    public sealed record GetLoadStateRequest(string LineCode, string PartNo) : IRequest<GetLoadStateResponse>;
}
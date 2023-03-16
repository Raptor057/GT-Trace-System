using GT.Trace.Common.CleanArch;

namespace GT.Trace.Changeover.App.UseCases.GetLine
{
    public sealed record GetLineByCodeRequest(string LineCode) : IRequest<GetLineResponse>;
}
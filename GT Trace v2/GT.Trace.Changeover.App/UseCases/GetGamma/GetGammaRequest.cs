using GT.Trace.Common.CleanArch;

namespace GT.Trace.Changeover.App.UseCases.GetGamma
{
    public sealed record GetGammaRequest(string LineCode, string PartNo, string Revision) : IRequest<GetGammaResponse>;
}
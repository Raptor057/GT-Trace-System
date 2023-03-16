using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetBom
{
    public sealed record GetBomRequest(string PartNo, string Revision) : IResultRequest<GetBomResponse>;
}
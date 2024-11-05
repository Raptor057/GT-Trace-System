using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.UpdateGama
{
    //public sealed record UpdateBomLineRequest(string PartNo, string LineCode): IRequest<UpdateBomLineResponse>;
    public sealed record UpdateBomLineRequest(string ogpartNo, string icpartNo, string oglineCode, string iclineCode) : IRequest<UpdateBomLineResponse>;
}

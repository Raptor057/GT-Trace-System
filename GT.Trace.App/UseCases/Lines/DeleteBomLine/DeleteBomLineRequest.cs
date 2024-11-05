using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.DeleteBomLine
{
    //public sealed record UpdateBomLineRequest(string ogpartNo, string icpartNo, string oglineCode, string iclineCode) : IRequest<UpdateBomLineResponse>;
    public sealed record DeleteBomLineRequest(string ogpartNo, string icpartNo, string oglineCode, string iclineCode) : IRequest<DeleteBomLineResponse>;
}

using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.GetLastMasterFolioByLine
{
    public sealed record GetLastMasterFolioByLineRequest(string LineCode, string LineName) : IRequest<GetLastMasterFolioByLineResponse>;
}
using GT.Trace.Common.CleanArch;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.GetLastMasterFolioByLine
{
    public sealed record GetLastMasterFolioByLineRequest(string LineCode, string LineName): IRequest<GetLastMasterFolioByLineResponse>;
}

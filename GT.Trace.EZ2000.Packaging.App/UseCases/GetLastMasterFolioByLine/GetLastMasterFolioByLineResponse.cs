using GT.Trace.Common.CleanArch;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.GetLastMasterFolioByLine
{
    public record GetLastMasterFolioByLineResponse(long? Folio, string LineCode, string LineName) : IResponse;
}

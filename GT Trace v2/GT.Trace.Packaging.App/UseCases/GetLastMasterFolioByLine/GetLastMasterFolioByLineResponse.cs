using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.GetLastMasterFolioByLine
{
    public record GetLastMasterFolioByLineResponse(long? Folio, string LineCode, string LineName) : IResponse;
}
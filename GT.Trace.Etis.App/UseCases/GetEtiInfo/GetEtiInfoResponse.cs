namespace GT.Trace.Etis.App.UseCases.GetEtiInfo
{
    public sealed record GetEtiInfoResponse(long EtiID, string EtiNo, string? ComponentNo, string? Revision, string? LotNo, bool IsLocked);
}
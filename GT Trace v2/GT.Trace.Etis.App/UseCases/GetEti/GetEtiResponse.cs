namespace GT.Trace.Etis.App.UseCases.GetEti
{
    public sealed record GetEtiResponse(long EtiID, string EtiNo, string? ComponentNo, string? Revision, string? LotNo, bool IsLocked);
}
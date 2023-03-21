namespace GT.Trace.EtiMovements.App.Dtos
{
    public sealed record EtiInfoDto(long EtiID, string EtiNo, string? ComponentNo, string? Revision, string? LotNo, bool IsLocked);
}
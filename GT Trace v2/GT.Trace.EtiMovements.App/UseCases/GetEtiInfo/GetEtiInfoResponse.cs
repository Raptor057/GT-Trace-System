namespace GT.Trace.EtiMovements.App.UseCases.GetEtiInfo
{
    public sealed record GetEtiInfoResponse(string EtiNo, string ComponentNo, string Revision, string? Status);
}
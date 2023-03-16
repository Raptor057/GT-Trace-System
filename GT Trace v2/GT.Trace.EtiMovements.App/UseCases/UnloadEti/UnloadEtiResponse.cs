namespace GT.Trace.EtiMovements.App.UseCases.UnloadEti
{
    public sealed record UnloadEtiResponse(string LineCode, string EtiNo, string ComponentNo, string PointOfUseCode);
}
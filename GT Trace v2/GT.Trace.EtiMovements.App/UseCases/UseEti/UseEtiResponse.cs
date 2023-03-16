namespace GT.Trace.EtiMovements.App.UseCases.UseEti
{
    public sealed record UseEtiResponse(string LineCode, string EtiNo, string ComponentNo, string PointOfUseCode);
}
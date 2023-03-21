namespace GT.Trace.EtiMovements.App.UseCases.LoadEti
{
    public sealed record LoadEtiResponse(string LineCode, string EtiNo, string ComponentNo, string PointOfUseCode);
}
namespace GT.Trace.EtiMovements.App.UseCases.ReturnEti
{
    public sealed record ReturnEtiResponse(string LineCode, string EtiNo, string PartNo, string ComponentNo, string PointOfUseCode, string OperatorNo, DateTime UtcTimeStamp);
}
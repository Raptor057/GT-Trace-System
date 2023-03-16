namespace GT.Trace.EtiMovements.Domain.Events
{
    public sealed record EtiLoadedEvent(string PointOfUseCode, string EtiNo, string ComponentNo, string LotNo, DateTime StartTime);
}
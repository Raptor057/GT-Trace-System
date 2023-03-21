namespace GT.Trace.Domain.Events
{
    public record MaterialUsedEvent(string LineCode, string PointOfUseCode, string EtiNo, string PartNo, string WorkOrderCode);
}
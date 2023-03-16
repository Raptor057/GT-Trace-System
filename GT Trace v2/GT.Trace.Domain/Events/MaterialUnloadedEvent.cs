namespace GT.Trace.Domain.Events
{
    public record MaterialUnloadedEvent(string LineCode, string PointOfUseCode, string EtiNo, string PartNo, string WorkOrderCode);
}
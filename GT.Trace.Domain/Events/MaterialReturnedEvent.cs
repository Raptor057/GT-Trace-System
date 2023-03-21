namespace GT.Trace.Domain.Events
{
    public record MaterialReturnedEvent(string LineCode, string PointOfUseCode, string EtiNo, string PartNo, string WorkOrderCode, bool IsDepleted);
}
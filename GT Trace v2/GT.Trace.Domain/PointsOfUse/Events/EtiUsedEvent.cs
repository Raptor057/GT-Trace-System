namespace GT.Trace.Domain.PointsOfUse.Events
{
    public record EtiUsedEvent(string EtiNo, DateTime UsageTime);
}
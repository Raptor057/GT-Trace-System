namespace GT.Trace.Domain.PointsOfUse.Events
{
    public record EtiUnloadedEvent(string EtiNo, DateTime ExpirationTime);
}
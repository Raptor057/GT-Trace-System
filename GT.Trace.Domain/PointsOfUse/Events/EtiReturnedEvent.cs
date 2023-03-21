namespace GT.Trace.Domain.PointsOfUse.Events
{
    public record EtiReturnedEvent(string EtiNo, bool IsDepleted, DateTime ExpirationTime);
}
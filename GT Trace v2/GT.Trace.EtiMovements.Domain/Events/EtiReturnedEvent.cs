namespace GT.Trace.EtiMovements.Domain.Events
{
    public sealed record EtiReturnedEvent(string EtiNo, DateTime EndTime);
}
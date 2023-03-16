namespace GT.Trace.EtiMovements.Domain.Events
{
    public sealed record EtiUnloadedEvent(string EtiNo, DateTime EndTime);
}

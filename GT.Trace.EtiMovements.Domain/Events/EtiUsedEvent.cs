namespace GT.Trace.EtiMovements.Domain.Events
{
    public sealed record EtiUsedEvent(string EtiNo, string? EtiToReplace, DateTime UsageTime);
}
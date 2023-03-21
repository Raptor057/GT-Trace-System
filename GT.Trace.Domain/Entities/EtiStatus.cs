namespace GT.Trace.Domain.Entities
{
    public record EtiStatus(string PointOfUseCode, DateTime EffectiveTime, DateTime? UsageTime, DateTime? ExpirationTime, bool IsDepleted);
}
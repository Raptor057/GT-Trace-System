namespace GT.Trace.Domain.PointsOfUse.Events
{
    public record EtiLoadedEvent(string EtiNo, string ComponentNo, string PointOfUseCode, DateTime EffectiveTime);
}
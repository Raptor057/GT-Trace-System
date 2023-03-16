namespace GT.Trace.App.UseCases.Lines.GetLoadState
{
    public sealed record PointOfUseEtiEntryDto(string PointOfUseCode, string EtiNo, string ComponentNo, DateTime EffectiveTime);
}
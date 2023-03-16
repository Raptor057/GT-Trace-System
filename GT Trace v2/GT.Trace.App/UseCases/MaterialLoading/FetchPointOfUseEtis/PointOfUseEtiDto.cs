namespace GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis
{
    public record PointOfUseEtiDto(long ID, string EtiNo, string ComponentNo, DateTime EffectiveTime, string PartNo, string WorkOrderCode);
}
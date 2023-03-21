namespace GT.Trace.Domain.Events
{
    public record MaterialLoadedEvent(string OperatorNo, string PointOfUseCode, string EtiNo, string PartNo, string? LotNo, long Folio, string WorkOrderCode, string Order, string OrderLine, string ComponentNo, string Comments)
    {
        public DateTime UtcTimeStamp => DateTime.UtcNow;
    }
}
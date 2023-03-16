namespace GT.Trace.EZ2000.Packaging.Domain.Events
{
    public record UnitPickedEvent(long UnitID, string PartNo, int SequenceNo, string WorkOrderCode, string LineName);
}

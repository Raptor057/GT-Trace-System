namespace GT.Trace.Packaging.Domain.Events
{
    public record UnitPickedEvent(long UnitID, string PartNo, int SequenceNo, string WorkOrderCode, string LineName);
}
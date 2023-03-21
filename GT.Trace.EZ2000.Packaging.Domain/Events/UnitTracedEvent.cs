namespace GT.Trace.EZ2000.Packaging.Domain.Events
{
    public record UnitTracedEvent(long UnitID, string PartNo, string WorkOrderCode, string LineName, int Operation, string ClientName);
}

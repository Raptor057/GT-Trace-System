namespace GT.Trace.Packaging.Domain.Events
{
    //public record UnitTracedEvent(long UnitID, string PartNo, string WorkOrderCode, string LineName, int Operation, string ClientName);
    public record UnitTracedEvent(long UnitID, string PartNo, string WorkOrderCode, string LineName, string LineCode, int Operation, string ClientName);
}
namespace GT.Trace.Packaging.Domain.Events
{
    public record UnitPackedEvent(int LineID, string LineCode, string LineName, string WorkOrderCode, string ClientName, string PartNo, string Revision, long UnitID, string JulianDay, bool IsPartial, long? MasterID, long? ApprovalID);
}
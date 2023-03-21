namespace GT.Trace.EZ2000.Packaging.Domain.Events
{
    public record PalletCompletedEvent(string ClientName, string PartNo, string Revision, string CustomerPartNo, string PartDescription, string PO,string LotNo, string ClientDescription, string LineName,
        bool IsClosed, int Quantity, string MasterType, bool WasPartial, bool IsPartial, string JulianDay, string ProductFamily, string WorkOrderCode, long ApprovalID, string? ApprovalUser, DateTime? ApprovalDate);

}

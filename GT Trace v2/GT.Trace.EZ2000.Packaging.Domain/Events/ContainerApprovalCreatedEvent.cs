namespace GT.Trace.EZ2000.Packaging.Domain.Events
{
    public record ContainerApprovalCreatedEvent(long ApprovalID, string LineName, string PartNo, string WorkOrderCode, string CustomerPartNo, string Revision, int PalletSize, int ContainerSize);
}

namespace GT.Trace.EtiMovements.Domain.Entities
{
    public sealed class WorkOrder
    {
        public WorkOrder(string partNo, string revision)
        {
            PartNo = partNo;
            Revision = revision;
        }

        public string PartNo { get; }

        public string Revision { get; }
    }
}
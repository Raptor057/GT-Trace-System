namespace GT.Trace.Infra.Entities
{
    internal class ProductionLogs
    {
        public long ID { get; set; }

        public string LineCode { get; set; } = "";

        public string PartNo { get; set; } = "";

        public string Revision { get; set; } = "";

        public string WorkOrderCode { get; set; } = "";

        public int Quantity { get; set; }

        public DateTime UtcTimeStamp { get; set; }
    }
}
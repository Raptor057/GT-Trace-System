namespace GT.Trace.Infra.Entities
{
    internal class PointOfUseEtis
    {
        public long ID { get; set; }

        public string PartNo { get; set; } = "";

        public string WorkOrderCode { get; set; } = "";

        public string PointOfUseCode { get; set; } = "";

        public string EtiNo { get; set; } = "";

        public string ComponentNo { get; set; } = "";

        public DateTime UtcEffectiveTime { get; set; }

        public DateTime? UtcUsageTime { get; set; }

        public DateTime? UtcExpirationTime { get; set; }

        public string Comments { get; set; } = "";

        public bool IsDepleted { get; set; }

        public int Size { get; set; }

        public int PackingCount { get; set; }
    }
}
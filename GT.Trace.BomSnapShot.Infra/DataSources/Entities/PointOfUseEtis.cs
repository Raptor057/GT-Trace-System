namespace GT.Trace.BomSnapShot.Infra.DataSources.Entities
{
    public class PointOfUseEtis
    {
        public long ID { get; set; }

        public string PointOfUseCode { get; set; } = "";

        public string EtiNo { get; set; } = "";

        public string ComponentNo { get; set; } = "";

        public DateTime UtcEffectiveTime { get; set; }

        public DateTime? UtcUsageTime { get; set; }

        public DateTime? UtcExpirationTime { get; set; }

        public bool IsDepleted { get; set; }

        public string LotNo { get; set; } = "";
    }
}

namespace GT.Trace.BomSnapShot.Infra.DataSources.Entities
{
    public class ComponentsSnapShot
    {
        public long ID { get; set; }

        public long SnapShotID { get; set; }

        public string? LineCode { get; set; }

        public string? PointOfUseCode { get; set; }

        public string? EtiNo { get; set; }

        public string? MasterEtiNo { get; set; }

        public string? ComponentNo { get; set; }

        public string? ComponentRev { get; set; }

        public string? ComponentDescription { get; set; }

        public string? LotNo { get; set; }

        public DateTime UtcTimeStamp { get; set; }

    }
}

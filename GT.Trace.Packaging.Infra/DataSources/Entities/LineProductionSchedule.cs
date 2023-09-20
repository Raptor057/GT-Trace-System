namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class LineProductionSchedule
    {
        public string LineCode { get; set; } = "";

        public string WorkOrderCode { get; set; } = "";

        public string PartNo { get; set; } = "";

        public int HourlyRate { get; set; }

        public DateTime UtcEffectiveTime { get; set; }

        public DateTime UtcExpirationTime { get; set; }

        public string Revision { get; set; } = "";

    }
}

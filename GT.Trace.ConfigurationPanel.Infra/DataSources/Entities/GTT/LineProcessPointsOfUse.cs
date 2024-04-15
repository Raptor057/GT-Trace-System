namespace GT.Trace.ConfigurationPanel.Infra.DataSources.Entities.GTT
{
    public class LineProcessPointsOfUse
    {
        public string? LineCode { get; set; }

        public string? ProcessID { get; set; }

        public string? PointOfUseCode { get; set; }

        public DateTime UtcEffectiveTime { get; set; }

        public DateTime UtcExpirationTime { get; set; }

    }
}

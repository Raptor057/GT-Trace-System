namespace GT.Trace.ConfigurationPanel.Infra.DataSources.Entities.GTT
{
    public class LinePointsOfUse
    {
        public string? LineCode { get; set; }

        public string? PointOfUseCode { get; set; }

        public bool IsDisabled { get; set; }

        public bool CanBeLoadedByOperations { get; set; }

    }
}

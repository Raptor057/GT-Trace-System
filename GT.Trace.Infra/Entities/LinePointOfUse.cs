namespace GT.Trace.Infra.Entities
{
    internal class LinePointOfUse
    {
        public string LineCode { get; set; } = "";

        public string PointOfUseCode { get; set; } = "";

        public bool IsDisabled { get; set; }

        public bool CanBeLoadedByOperations { get; set; }
    }
}
namespace GT.Trace.EZ2000.Packaging.Infra.DataSources.Entities
{
    internal class LineRouting
    {
        public long LineRoutingID { get; set; }

        public string LineCode { get; set; } = "";

        public string ProcessNo { get; set; } = "";

        public string PrevProcessNo { get; set; } = "";

        public bool IsEnabled { get; set; }
    }
}
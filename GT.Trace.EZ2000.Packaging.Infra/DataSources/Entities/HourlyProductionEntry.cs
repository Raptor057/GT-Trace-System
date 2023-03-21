namespace GT.Trace.EZ2000.Packaging.Infra.DataSources.Entities
{
    internal class HourlyProductionEntry
    {
        public int hour { get; set; }

        public string? part_no { get; set; }

        public int? required_qty { get; set; }

        public int? quantity { get; set; }
    }
}
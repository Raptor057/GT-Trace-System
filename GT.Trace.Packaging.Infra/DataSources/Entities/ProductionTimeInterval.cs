namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class ProductionTimeInterval
    {
        public string LineCode { get; set; } = "";

        public string Name { get; set; } = "";

        public bool IsPastDue { get; set; }

        public bool IsCurrent { get; set; }

        public int EffectiveHourlyRequirement { get; set; }

        public string PartNo { get; set; } = "";

        public int? Quantity { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public decimal? Pph { get; set; }

        public int Headcount { get; set; }
    }
}
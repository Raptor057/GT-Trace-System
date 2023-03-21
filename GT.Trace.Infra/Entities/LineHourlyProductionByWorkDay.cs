namespace GT.Trace.Infra.Entities
{
    internal class LineHourlyProductionByWorkDay
    {
        public string LineCode { get; set; } = "";

        public DateTime WorkDayDate { get; set; }

        public string IntervalName { get; set; } = "";

        public int HourlyRequirement { get; set; }

        public string? PartNo { get; set; }

        public int? Quantity { get; set; }
    }
}
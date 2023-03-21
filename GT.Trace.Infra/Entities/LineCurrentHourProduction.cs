namespace GT.Trace.Infra.Entities
{
    internal class LineCurrentHourProduction
    {
        public string LineCode { get; set; } = "";

        public string Interval { get; set; } = "";

        public int ActualQuantity { get; set; }

        public int ExpectedQuantity { get; set; }

        public int Forecast { get; set; }

        public decimal ExpectedRate { get; set; }

        public decimal ActualRate { get; set; }

        public int Requirement { get; set; }
    }
}
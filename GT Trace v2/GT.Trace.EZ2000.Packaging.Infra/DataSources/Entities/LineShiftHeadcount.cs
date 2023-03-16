namespace GT.Trace.EZ2000.Packaging.Infra.DataSources.Entities
{
    public sealed class LineShiftHeadcount
    {
        public int ShiftScheduleID { get; set; }

        public string LineCode { get; set; } = "";

        public DateTime Moment { get; set; }

        public string ShiftName { get; set; } = "";

        public DateTime ShiftStartTime { get; set; }

        public DateTime ShiftEndTime { get; set; }

        public int Headcount { get; set; }
    }
}
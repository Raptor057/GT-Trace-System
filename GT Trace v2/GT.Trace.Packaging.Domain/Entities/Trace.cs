namespace GT.Trace.Packaging.Domain.Entities
{
    public class Trace
    {
        public Trace(string lineName, DateTime utcScanMoment)
            : this(ID.New(), lineName, utcScanMoment)
        { }

        public Trace(ID id, string lineName, DateTime utcScanMoment)
        {
            ID = id;
            LineName = lineName;
            UtcScanMoment = utcScanMoment;
        }

        public ID ID { get; }

        public string LineName { get; }

        public DateTime UtcScanMoment { get; }
    }
}
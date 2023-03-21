namespace GT.Trace.Domain.Entities
{
    public sealed class Label
    {
        public Label(long unitID, Part part, string clientPartNo, string julianDay)
        {
            UnitId = unitID;
            Part = part;
            ClientPartNo = clientPartNo;
            JulianDay = julianDay;
        }

        public long UnitId { get; }

        public Part Part { get; }

        public string ClientPartNo { get; }

        public string JulianDay { get; }
    }
}
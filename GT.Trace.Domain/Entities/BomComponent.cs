namespace GT.Trace.Domain.Entities
{
    public sealed class BomComponent
    {
        public BomComponent(string pointOfUseCode, string compNo, string compRev, string compDesc, int capacity)
        {
            PointOfUseCode = pointOfUseCode;
            CompNo = compNo;
            Capacity = capacity;
            CompRev = compRev;
            CompDesc = compDesc;
        }

        public string PointOfUseCode { get; }

        public string CompNo { get; }

        public int Capacity { get; }

        public string CompRev { get; }

        public string CompDesc { get; }
    }
}
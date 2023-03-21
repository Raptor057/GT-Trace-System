namespace GT.Trace.Domain.PointsOfUse.Entities
{
    public sealed class BomItem
    {
        public BomItem(string componentNo, string pointOfUseCode, int capacity)
        {
            ComponentNo = componentNo;
            PointOfUseCode = pointOfUseCode;
            Capacity = capacity;
        }

        public string ComponentNo { get; }

        public string PointOfUseCode { get; }

        public int Capacity { get; }
    }
}
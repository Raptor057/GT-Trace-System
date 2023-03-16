namespace GT.Trace.Domain.PointsOfUse.Entities
{
    public sealed class Bom
    {
        public Bom(string partNo, string revision, List<BomItem> items)
        {
            PartNo = partNo;
            Revision = revision;
            Items = items;
        }

        public string PartNo { get; }

        public string Revision { get; }

        public IReadOnlyList<BomItem> Items { get; }

        public BomItem? Component { get; }

        public bool HasPointOfUse(string code) => Items.Where(item => item.PointOfUseCode == code).Any();

        public bool TryGetComponent(string componentNo, string pointOfUseCode, out BomItem? component)
        {
            component = Items.SingleOrDefault(item => item.ComponentNo == componentNo && item.PointOfUseCode == pointOfUseCode);
            return component != null;
        }
    }
}
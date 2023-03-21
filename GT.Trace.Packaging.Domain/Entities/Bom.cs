namespace GT.Trace.Packaging.Domain.Entities
{
    public class Bom
    {
        public Bom(List<BomComponent> components)
        {
            Components = components;
            Label = components.Find(item => item.Component[..2] == "55" && item.Description.Contains("label", StringComparison.OrdinalIgnoreCase)) ?? new BomComponent("", "", "", "", "");
        }

        public IReadOnlyCollection<BomComponent> Components { get; }

        public BomComponent Label { get; }
    }
}
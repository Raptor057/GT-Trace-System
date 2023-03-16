namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    public class Bom
    {
        /// <summary>
        /// Averiguar donde o como funciona este metodo.
        /// </summary>
        /// <param name="components"></param>
        public Bom(List<BomComponent> components)
        {
            Components = components;
            Label = components.Find(item => item.Component[..2] == "55" && item.Description.Contains("label", StringComparison.OrdinalIgnoreCase)) ?? new BomComponent("", "", "", "", "");
        }

        public IReadOnlyCollection<BomComponent> Components { get; }
        public BomComponent Label { get; }
    }
}

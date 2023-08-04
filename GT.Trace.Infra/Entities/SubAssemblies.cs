namespace GT.Trace.Infra.Entities
{
    public class SubAssemblies
    {
        public long SubAssemblyID { get; set; }

        public string? LineCode { get; set; }

        public string? WorkOrderCode { get; set; }

        public string? ComponentNo { get; set; }

        public string? Revision { get; set; }

        public int Quantity { get; set; }

        public bool IsDisabled { get; set; }

        public DateTime UtcCreationTime { get; set; }
    }
}

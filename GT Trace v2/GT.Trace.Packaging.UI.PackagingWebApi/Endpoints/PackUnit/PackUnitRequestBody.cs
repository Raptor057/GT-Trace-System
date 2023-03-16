namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.PackUnit
{
    public class PackUnitRequestBody
    {
        public string? ScannerInput { get; set; }

        public string? LineCode { get; set; }

        public int? PalletSize { get; set; }

        public int? ContainerSize { get; set; }

        public string? PoNumber { get; set; }
    }
}
namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.JoinFramelessMotors
{
    public class JoinFramelessMotorsRequestBody
    {
        public string? ScannerInputUnitID { get; set; }
        public string? ScannerInputComponentID { get; set; }
        public string? LineCode { get; set; }
        public string? PartNo { get; set; }
    }
}

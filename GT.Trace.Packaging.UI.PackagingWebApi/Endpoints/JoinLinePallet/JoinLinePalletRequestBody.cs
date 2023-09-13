namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.JoinPalletQR
{
    public class JoinLinePalletRequestBody
    {
        public string? ScannerInputUnitID { get; set; }
        public string? ScannerInputPalletID { get; set; }
        public string? LineCode { get; set; }
        public int IsEnable { get; set; }
    }
}

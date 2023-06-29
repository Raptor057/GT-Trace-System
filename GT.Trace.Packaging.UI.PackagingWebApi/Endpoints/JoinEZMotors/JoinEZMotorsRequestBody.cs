
namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.JoinEZMotors
{
    public class JoinEZMotorsRequestBody
    {
        public string? ScannerInputUnitID { get; set; }
        public string? ScannerOutputMotorID1 { get; set; }
        public string? ScannerOutputMotorID2 { get; set; }
        public int isEnable { get; set; }
    }
}

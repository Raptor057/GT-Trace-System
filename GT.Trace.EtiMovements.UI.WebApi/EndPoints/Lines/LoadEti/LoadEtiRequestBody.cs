namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Lines.LoadEti
{
    public class LoadEtiRequestBody
    {
        public string? EtiInput { get; set; }

        public string? PointOfUseCode { get; set; }

        public bool IgnoreCapacity { get; set; }
    }
}
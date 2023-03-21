namespace GT.Trace.UI.CegidRadioWebApi.EndPoints.ReportFabricationControl
{
    public class CreateFabricationControlEntryRequest
    {
        public string? PartNo { get; set; }

        public string? Revision { get; set; }

        public string? WorkOrderCode { get; set; }

        public int? Quantity { get; set; }

        public long? EtiID { get; set; }

        public string? DepoCode { get; set; }

        public bool OrderIsClosed { get; set; }

        public string? LocationCode
        {
            get
            {
                return DepoCode;
            }
            set
            {
                DepoCode = value;
            }
        }
    }
}
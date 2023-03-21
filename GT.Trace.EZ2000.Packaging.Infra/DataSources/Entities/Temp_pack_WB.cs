namespace GT.Trace.EZ2000.Packaging.Infra.DataSources.Entities
{
    public class Temp_pack_WB
    {
        public string linea { get; set; } = "";

        public string modelo { get; set; } = "";

        public string telesis { get; set; } = "";

        public DateTime fecha { get; set; }

        public string num_p { get; set; } = "";

        public string LEVEL { get; set; } = "";

        public bool? IS_partial { get; set; }

        public string Master_id { get; set; } = "";

        public long? Aproved { get; set; }
    }
}
namespace GT.Trace.EZ2000.Packaging.Infra.DataSources.Entities
{
    public class TZ_QCParam_App
    {
        public int Id { get; set; }

        public int? PARAM_INI { get; set; }

        public int? PARAM_FIN { get; set; }

        public string FAMILIA { get; set; } = "";

        public string PACK_TYPE { get; set; } = "";

        public int? RW_PARAM_INI { get; set; }

        public int? RW_PARAM_FIN { get; set; }
    }
}
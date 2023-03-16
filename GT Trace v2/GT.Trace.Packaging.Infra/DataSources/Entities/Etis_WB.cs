namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class Etis_WB
    {
        public long ID { get; set; }

        public string SET_ID { get; set; } = "";

        public string eti_no { get; set; } = "";

        public long eti_001 { get; set; }

        public string component { get; set; } = "";

        public string rev_cc { get; set; } = "";

        public string lote { get; set; } = "";

        public string fecha { get; set; } = "";

        public string linea { get; set; } = "";

        public string puesto_no { get; set; } = "";

        public string NP_FINAL { get; set; } = "";

        public string MODELO { get; set; } = "";

        public bool? need_val { get; set; }

        public bool? was_val { get; set; }

        public bool? status { get; set; }

        public DateTime creation_time { get; set; }

        public string source { get; set; } = "";

        public int? packing_count { get; set; }
    }
}
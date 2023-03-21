namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class Tbl_qc_aproved_list
    {
        public long Id { get; set; }

        public string linea { get; set; } = "";

        public string part_num { get; set; } = "";

        public string codewo { get; set; } = "";

        public int qty { get; set; }

        public bool? is_approved { get; set; }

        public string fecha_app { get; set; } = "";

        public string user_app { get; set; } = "";

        public string fecha { get; set; } = "";

        public long? ID_MASTER { get; set; }

        public string TM_verificadas { get; set; } = "";

        public string PN_Cliente { get; set; } = "";

        public string Rev { get; set; } = "";

        public string lote { get; set; } = "";

        public int? qty_Final { get; set; }

        public int? qty_cont { get; set; }

        public bool? is_finished { get; set; }
    }
}
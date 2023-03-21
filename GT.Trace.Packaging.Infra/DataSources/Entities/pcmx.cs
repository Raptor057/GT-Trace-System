namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class pcmx
    {
        public int Id { get; set; }

        public string UBICACION { get; set; } = "";

        public string PCNAME { get; set; } = "";

        public string LINE { get; set; } = "";

        public string PROCESSNAME { get; set; } = "";

        public string PRINTER1 { get; set; } = "";

        public string PRINTER2 { get; set; } = "";

        public string PRINTER3 { get; set; } = "";

        public string PRINTER_DEFAULT { get; set; } = "";

        public bool? is_blocked { get; set; }

        public bool? is_ready_TZ { get; set; }

        public bool? is_ready_TZZ { get; set; }

        public bool? IS_PACKME { get; set; }

        public bool? Can_Chg_Line { get; set; }

        public bool? Can_pick_qctest { get; set; }

        public bool? Can_Save_traza { get; set; }

        public bool? Can_autoload { get; set; }

        public bool? Can_Cartlst_wh { get; set; }

        public bool? Can_Val_custpn { get; set; }

        public bool? Can_Val_Rev { get; set; }

        public bool? Can_Val_Benchtest { get; set; }

        public bool? Config_mode_traca { get; set; }

        public DateTime? Last_date_run { get; set; }

        public bool is_enabled { get; set; }


    }
}
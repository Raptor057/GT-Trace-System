namespace GT.Trace.Infra.Entities
{
    internal class Tbl_Point_use
    {
        public long Id { get; set; }

        public string linea { get; set; } = "";

        public string codew { get; set; } = "";

        public string NP_final { get; set; } = "";

        public string Folio { get; set; } = "";

        public string Linea_Orden { get; set; } = "";

        public string Orden { get; set; } = "";

        public string ETI_no { get; set; } = "";

        public string LOTE { get; set; } = "";

        public string Punto_uso { get; set; } = "";

        public string Operador { get; set; } = "";

        public string fecha { get; set; } = "";

        public string hora { get; set; } = "";

        public string Componente { get; set; } = "";

        public bool? is_used { get; set; }

        public string hora_SCAN { get; set; } = "";

        public string fecha_uso { get; set; } = "";

        public string puesto_uso { get; set; } = "";

        public string fecha_retorno { get; set; } = "";

        public bool? is_finished { get; set; }

        public string comments { get; set; } = "";
    }
}
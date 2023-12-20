namespace GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources.Entities
{
public class tbl_rmnc
    {
        //Esta tabla se usa para auditor de calida de produccion de WB
        public long Id { get; set; }

        public string RMNC_ID { get; set; }

        public DateTime? fecha_c { get; set; }

        public string part_number { get; set; }

        public string rev { get; set; }

        public string description { get; set; }

        public int qty_lote { get; set; }

        public int qty_muestra { get; set; }

        public int rej_muestra { get; set; }

        public int porc_rej { get; set; }

        public string tipo_rej { get; set; }

        public bool medio_detec_prod { get; set; }

        public bool segregar_hold { get; set; }

        public string Transfer_no { get; set; }

        public string area_detecta { get; set; }

        public string area_reporta { get; set; }

        public string dib_ref { get; set; }

        public string dib_esp { get; set; }

        public string dib_actual { get; set; }

        public string dim_ref { get; set; }

        public string dim_esp { get; set; }

        public string dim_actual { get; set; }

        public string apar_ref { get; set; }

        public string apar_esp { get; set; }

        public string apar_actual { get; set; }

        public string proc_ref { get; set; }

        public string proc_esp { get; set; }

        public string proc_actual { get; set; }

        public string func_ref { get; set; }

        public string func_esp { get; set; }

        public string func_actual { get; set; }

        public string comments { get; set; }

        public DateTime fecha { get; set; }

        public string elaboro { get; set; }

        public bool? is_open { get; set; }

        public int? avance { get; set; }

        public bool is_closed { get; set; }

    }
}

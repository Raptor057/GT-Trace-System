namespace GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources.Entities
{
    public class Master_labels_WB
    {
        public long Id { get; set; }

        public string fecha { get; set; }

        public string Hora { get; set; }

        public int qty { get; set; }

        public string line { get; set; }

        public string FAMILIA { get; set; }

        public string modelo { get; set; }

        public string part_num { get; set; }

        public string rev { get; set; }

        public string description { get; set; }

        public string codew { get; set; }

        public string customer { get; set; }

        public string cust_pn { get; set; }

        public string po_num { get; set; }

        public string LOTES { get; set; }

        public string master_type { get; set; }

        public string juliano { get; set; }

        public bool was_partial { get; set; }

        public bool is_partial { get; set; }

        public bool Closed { get; set; }

        public bool aprov { get; set; }

        public string aprov_user { get; set; }

        public string aprov_date { get; set; }

        public long? aprov_ID { get; set; }

        public bool? shipped { get; set; }

        public string ship_user { get; set; }

        public string ship_date { get; set; }

        public bool? reworked { get; set; }

        public bool? reworked_ok { get; set; }

        public bool? ateq_val { get; set; }

        public string Reject_comments { get; set; }

        public bool? Reported_unaprov { get; set; }

        public bool? Shipped_wh { get; set; }

        public string ship_user_WH { get; set; }

        public string ship_date_WH { get; set; }

        public bool? is_active { get; set; }

        public string comments { get; set; }

        public bool? blocked { get; set; }

        public DateTime? date_blocked { get; set; }

        public string RMNC { get; set; }

        public DateTime? date_released { get; set; }

    }
}

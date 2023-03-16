namespace GT.Trace.Etis.Infra.Entities
{
    public class SubAssemblyEti
    {
        public long ID { get; set; }

        public string ComponentNo { get; set; }

        public string Revision { get; set; }

        public DateTime UtcCreationTime { get; set; }
    }

    public class pro_eti001
    {
        public int id { get; set; }

        public string part_number { get; set; }

        public int? std_pack { get; set; }

        public string lot { get; set; }

        public int? qty { get; set; }

        public int? initial_qty { get; set; }

        public DateTime? created_at { get; set; }

        public string current_location_id { get; set; }

        public DateTime? date_wh_loc { get; set; }

        public bool? is_open { get; set; }

        public string scale_type { get; set; }

        public bool? is_verified { get; set; }

        public bool? is_approved { get; set; }

        public string type { get; set; }

        public string reference { get; set; }

        public string supplier { get; set; }

        public string ref_ext { get; set; }

        public string rev { get; set; }

        public string rev_cc { get; set; }

        public string pre_rev { get; set; }

        public DateTime? date_changed_rev { get; set; }

        public int? id_user_changed_rev { get; set; }

        public string reason_changed_rev { get; set; }

        public string pre_ref { get; set; }

        public DateTime? date_changed_ref { get; set; }

        public int? id_user_changed_ref { get; set; }

        public string reason_changed_ref { get; set; }

        public string description { get; set; }

        public string next_op { get; set; }

        public decimal? sample_pc { get; set; }

        public decimal? sample_tara { get; set; }

        public decimal? sample_container { get; set; }

        public bool? manual_qty { get; set; }

        public string reason_manual { get; set; }

        public int? reason_manual_user { get; set; }

        public int? machine { get; set; }

        public int? shift { get; set; }

        public int? created_by { get; set; }

        public DateTime? last_mod_date { get; set; }

        public int? last_mod_user { get; set; }

        public DateTime? prod_report_date { get; set; }

        public int? prod_report_user { get; set; }

        public string prod_report_reference { get; set; }

        public bool? Blocked { get; set; }

        public DateTime? date_blocked { get; set; }

        public string RMNC { get; set; }

        public DateTime? date_released { get; set; }

        public bool? corona_approved { get; set; }

        public string corona_app_date { get; set; }

        public string traetment_req { get; set; }

        public bool? was_printed { get; set; }

        public bool? cert_ontime { get; set; }

        public string description2 { get; set; }

        public int packing_count { get; set; }
    }
}
namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class pro_production
    {
        public int id { get; set; }

        public int id_line { get; set; }

        public bool? is_stoped { get; set; }

        public bool? is_running { get; set; }

        public bool? is_finished { get; set; }

        public string order { get; set; } = "";

        public string line { get; set; } = "";

        public string subindex { get; set; } = "";

        public string codew { get; set; } = "";

        public string part_number { get; set; } = "";

        public string rev { get; set; } = "";

        public string part_desc { get; set; } = "";

        public decimal? weight_unit { get; set; }

        public string ratio { get; set; } = "";

        public int? std_pack { get; set; }

        public string ref_ext { get; set; } = "";

        public int? client_code { get; set; }

        public string client_name { get; set; } = "";

        public int? current_qty { get; set; }

        public int? target_qty { get; set; }

        public DateTime start_date { get; set; }

        public DateTime? end_date { get; set; }

        public int? created_by { get; set; }

        public int? end_by { get; set; }

        public DateTime? end_at { get; set; }

        public int? dblid { get; set; }

        public DateTime last_update_time { get; set; }
    }
}
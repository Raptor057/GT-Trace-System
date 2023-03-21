namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class TZ_tblWBTest_counters
    {
        public long Id { get; set; }

        public int? id_ssFam { get; set; }

        public string NP { get; set; } = "";

        public string REV { get; set; } = "";

        public int? Counter { get; set; }

        public int? tot_test { get; set; }

        public bool? is_active { get; set; }

        public DateTime? fecha { get; set; }
    }
}
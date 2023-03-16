namespace GT.Trace.Infra.Entities
{
    internal class Tbl_events_log
    {
        public Tbl_events_log(string errorMessage, string category, DateTime timeStamp, string station)
        {
            error_msg = errorMessage;
            categoria = category;
            fecha = timeStamp.ToString("dd-MMM-yyyy HH:mm:ss");
            PCNAME = station;
        }

        public long Id { get; set; }

        public string error_msg { get; set; }

        public string categoria { get; set; }

        public string fecha { get; set; }

        public string PCNAME { get; set; }
    }
}
namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class REFEXT
    {
        public string RFKTSOC { get; set; } = "";

        public string RFKTCODART { get; set; } = "";

        public string RFKTCOMART { get; set; } = "";

        public string RFKTTYPE { get; set; } = "";

        public string RFKTCODE { get; set; } = "";

        public string RFCTREF { get; set; } = "";

        public string RFCTLIB01 { get; set; } = "";

        public string RFCTLIB02 { get; set; } = "";

        public string RFCTTOP01 { get; set; } = "";

        public string RFCTTOP02 { get; set; } = "";

        public decimal? RFCNNUM01 { get; set; }

        public string RFCJMAJPU { get; set; } = "";

        public string RFCTUNICDE { get; set; } = "";

        public string RFCTUNIFAC { get; set; } = "";

        public int? RFCNREAPRO { get; set; }

        public string RFCTJOURAC { get; set; } = "";

        public decimal? RFCNPOUNI { get; set; }

        public string RFCTTRANSP { get; set; } = "";

        public decimal? RFCNPCECON { get; set; }

        public string RFCTPRIORI { get; set; } = "";

        public string RFCTCOMMEN { get; set; } = "";

        public decimal? RFCNMINCDE { get; set; }

        public decimal? RFCNQTECO { get; set; }

        public string RFCTTEXT1 { get; set; } = "";

        public decimal? RFCNLIBRE1 { get; set; }

        public decimal? RFCNQTUC1 { get; set; }

        public string RFCTEMBUC1 { get; set; } = "";

        public decimal? RFCNQTUC2 { get; set; }

        public string RFCTEMBUC2 { get; set; } = "";

        public decimal? RFCNQTUC3 { get; set; }

        public string RFCTEMBUC3 { get; set; } = "";

        public decimal? RFCNQTUC4 { get; set; }

        public string RFCTEMBUC4 { get; set; } = "";

        public int? RFCNNBUCNI { get; set; }

        public string RFCTMODUC1 { get; set; } = "";

        public string RFCTMODUC2 { get; set; } = "";

        public string RFCTMODUC3 { get; set; } = "";

        public string RFCTFIREWA { get; set; } = "";

        public string PackType => RFCTCOMMEN?.Trim() ?? "";

        public string PO => RFCTLIB01;
    }
}
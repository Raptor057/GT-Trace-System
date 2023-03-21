namespace GT.Trace.Infra.Entities
{
    internal class pro_subeti
    {
        public long Id { get; set; }

        public string eti1 { get; set; } = "";

        public string lot1 { get; set; } = "";

        public string NP1 { get; set; } = "";

        public string rev1 { get; set; } = "";

        public string eti2 { get; set; } = "";

        public string lot2 { get; set; } = "";

        public string NP2 { get; set; } = "";

        public string rev2 { get; set; } = "";

        public string maquina { get; set; } = "";

        public int turno { get; set; }

        public string next_oper { get; set; } = "";

        public int qty { get; set; }

        public string empleado { get; set; } = "";

        public string fecha { get; set; } = "";

        public bool? is_active { get; set; }

        public int packing_count { get; set; }
    }
}
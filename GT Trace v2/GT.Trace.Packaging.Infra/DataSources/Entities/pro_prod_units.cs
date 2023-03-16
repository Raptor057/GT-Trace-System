using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class pro_prod_units
    {
        public int id { get; set; }

        public string letter { get; set; } = "";

        public string comments { get; set; } = "";

        public string modelo { get; set; } = "";

        public string codew { get; set; } = "";

        public string Tipo { get; set; } = "";

        public string Familia { get; set; } = "";

        public int? Prod_Hour { get; set; }

        public int? prod_hour2 { get; set; }

        public int? week_demand { get; set; }

        public int? Turnos { get; set; }

        public int? Ini_1er_Turno { get; set; }

        public int? Fin_1er_Turno { get; set; }

        public int? min_1er_desc { get; set; }

        public int? hora_1er_desc { get; set; }

        public int? min_2do_desc { get; set; }

        public int? hora_2do_desc { get; set; }

        public float? hrs_disp { get; set; }

        public int? hora_Rev1 { get; set; }

        public int? min_rev { get; set; }

        public int? Ini_2do_Turno { get; set; }

        public int? Fin_2do_Turno { get; set; }

        public int? min_1er_desc2 { get; set; }

        public int? hora_1er_desc2 { get; set; }

        public int? min_2do_desc2 { get; set; }

        public int? hora_2do_desc2 { get; set; }

        public float? hrs_disp2 { get; set; }

        public int? hora_Rev2 { get; set; }

        public int? min_rev2 { get; set; }

        public int? Ini_3er_Turno { get; set; }

        public int? Fin_3er_Turno { get; set; }

        public int? min_1er_desc3 { get; set; }

        public int? hora_1er_desc3 { get; set; }

        public int? min_2do_desc3 { get; set; }

        public int? hora_2do_desc3 { get; set; }

        public float? hrs_disp3 { get; set; }

        public int? hora_Rev3 { get; set; }

        public int? min_rev3 { get; set; }

        public int? no_oper { get; set; }

        public DateTime created_at { get; set; }

        public int? PROD_HOUR_MON { get; set; }

        public string familia_qc { get; set; } = "";

        public string active_revision { get; set; } = "";
    }
}
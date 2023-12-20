using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources.Entities
{
    public class TBL_RMNC_ETI
    {
        //En la UI de Trazability esta tabla es la que se usa en el apartado de Disposicion
        public long Id { get; set; }

        public string RMNC_ID { get; set; }

        public string ETI_NO { get; set; }

        public string LOT { get; set; }

        public bool? IS_ETI { get; set; }

        public int QTY { get; set; }

        public int? NEW_QTY_RW { get; set; }

        public int NEW_QTY { get; set; }

        public int SCRAP_QTY { get; set; }

        public string SCRAP_TICKET { get; set; }

        public string TRANSFER_TICKET { get; set; }

        public string COMMENTS { get; set; }

        public bool IS_RELEASED { get; set; }

        public decimal costo_u { get; set; }

        public decimal costo_t { get; set; }

    }
}

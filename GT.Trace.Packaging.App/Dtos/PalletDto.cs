﻿namespace GT.Trace.Packaging.App.Dtos
{
    public class PalletDto
    {
        public string? LineName { get; set; }

        public bool? IsAteq { get; set; }

        public bool IsPartial { get; set; }

        public string? PurchaseOrderNo { get; set; }

        public string? JulianDate { get; set; }

        public string? Customer { get; set; }

        public string? CustomerPartNo { get; set; }

        public string? PartNo { get; set; }

        public string? Revision { get; set; }

        public string? PartDescription { get; set; }

        public DateTime Date { get; set; }

        public string? Approver { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public long? MasterID { get; set; }

        public int Quantity { get; set; }
        
        //Se Agrega campo Origen 3/27/2023
        public string? Origen { get; set; }

        //Se Agrega campo Www 4/16/2024
        public string? Www { get; set; }
    }
}
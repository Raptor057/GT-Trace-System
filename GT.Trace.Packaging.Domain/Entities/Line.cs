namespace GT.Trace.Packaging.Domain.Entities
{
    public record Line(
        int ID, string Code, string Name, int Headcount, Pallet Pallet, WorkOrder WorkOrder, Picking Picking, Bom Bom, Part LoadedPart, int QcUnapprovedPalletWarnLimit, int QcUnapprovedPalletStopLimit, IEnumerable<HourlyLineProcution> HourlyProduction, string[] ValidPackagingSources)
    {
        public bool QcContainerApprovalInWarning => Pallet.Quantity == (int)Math.Ceiling(QcUnapprovedPalletStopLimit / 2.0);

        public bool IsChangeOverRequired => string.Compare(LoadedPart.Number, WorkOrder.Part.Number, true) != 0;

        public bool QcContainerApprovalIsRequired => Pallet.Quantity >= QcUnapprovedPalletStopLimit && !(Pallet.Approval?.IsApproved ?? false); //aqui se define cuanto pide liberacion

        public bool POIsRequired => Pallet.ContainerTypeIsBox && !WorkOrder.PO.IsValid;
    }
}
namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    /// <summary>
    /// Parametros que recibe el record Line
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="Code"></param>
    /// <param name="Name"></param>
    /// <param name="HeadCount"></param>
    /// <param name="Pallet"></param>
    /// <param name="WorkOrder"></param>
    /// <param name="Picking"></param>
    /// <param name="Bom"></param>
    /// <param name="LoadedPart"></param>
    /// <param name="QcUnapprovedPalletWarnLimit"></param>
    /// <param name="QcUnapprovedPalletStopLimit"></param>
    /// <param name="HourlyProduction"></param>
    /// <param name="ValidPackagingSources"></param>
    public record Line(
        int ID, string Code, string Name, int HeadCount, Pallet Pallet, WorkOrder WorkOrder, Picking Picking, Bom Bom, Part LoadedPart, int QcUnapprovedPalletWarnLimit, int QcUnapprovedPalletStopLimit, IEnumerable<HourlyLineProcution> HourlyProduction, string[] ValidPackagingSources)
    {
        /// <summary>
        /// hace que la cantidad del pallet se trunque a un numero entero. 
        /// </summary>
        public bool QcContainerApprovalInWarning => Pallet.Quantity == (int)Math.Ceiling(QcUnapprovedPalletStopLimit / 2.0);

        /// <summary>
        /// compara el numero de parte cargado con el numero de parte de la orden de trabajo.
        /// </summary>
        public bool IsChangeOverRequired => string.Compare(LoadedPart.Number, WorkOrder.Part.Number, true) != 0;

        /// <summary>
        /// valida si es requerida la aprovacion de calidad 
        /// </summary>
        public bool QcContainerApprovalIsRequired => Pallet.Quantity >= QcUnapprovedPalletStopLimit && !(Pallet.Approval?.IsApproved ?? false);

        /// <summary>
        /// Valida si el tipo de contenedor es ina caja y valida si la orden de compra es valida.
        /// </summary>
        public bool POIsRequired => Pallet.ContainerTypeIsBox && !WorkOrder.PO.IsValid;
    }
}

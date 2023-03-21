namespace GT.Trace.Packaging.App.UseCases.LoadPackState.Dtos
{
    public record PackStateDto(string Name, string LineCode, string Family, int Headcount, PartDto LoadedPart, WorkOrderDto WorkOrder, StationDto Station, PalletDto Pallet,
        BomLabelDto BomLabel,
        ApprovalDto Approval,
        PickingDto Picking,
        IEnumerable<HourlyProductionItemDto> HourlyProduction);
}
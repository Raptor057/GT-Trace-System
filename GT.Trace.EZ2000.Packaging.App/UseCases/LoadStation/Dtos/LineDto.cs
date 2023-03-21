namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadStation.Dtos
{
    public record LineDto(string Name, string Family, PartDto LoadedPart, WorkOrderDto WorkOrder, StationDto Station, PalletDto Pallet, BomLabelDto BomLabel);
}
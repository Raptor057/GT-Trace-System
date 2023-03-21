namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadStation.Dtos
{
    public record WorkOrderDto(string Code, int Size, int Quantity, string PO, MasterTypeDto MasterType, PackTypeDto PackType, PartDto Part, ClientDto Client);
}
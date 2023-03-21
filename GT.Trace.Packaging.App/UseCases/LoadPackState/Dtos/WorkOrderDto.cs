namespace GT.Trace.Packaging.App.UseCases.LoadPackState.Dtos
{
    public record WorkOrderDto(string Code, int Size, int Quantity, string PO, MasterTypeDto MasterType, PackTypeDto PackType, PartDto Part, ClientDto Client);
}
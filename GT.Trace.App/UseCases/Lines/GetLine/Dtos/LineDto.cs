namespace GT.Trace.App.UseCases.Lines.GetLine.Dtos
{
    public sealed record LineDto(int ID, string Code, string Name, string ActiveWorkOrderCode, PartDto ActivePart, WorkOrderDto WorkOrder, IEnumerable<PointOfUseDto> PointsOfUse, bool OutputIsSubAssembly);
}
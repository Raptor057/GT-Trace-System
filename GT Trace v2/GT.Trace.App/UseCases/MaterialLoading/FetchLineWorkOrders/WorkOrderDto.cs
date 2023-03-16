namespace GT.Trace.App.UseCases.MaterialLoading.FetchLineWorkOrders
{
    public record WorkOrderDto(int ID, string Code, string PartNo, string Revision, string Order, string Line, string Client);
}
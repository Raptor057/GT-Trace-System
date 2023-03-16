namespace GT.Trace.App.UseCases.MaterialLoading.FetchLineWorkOrders
{
    public sealed record FetchLineWorkOrdersResponse(IEnumerable<WorkOrderDto> WorkOrders);
}
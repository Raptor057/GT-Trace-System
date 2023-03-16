namespace GT.Trace.App.UseCases.MaterialLoading.FetchLineWorkOrders
{
    public interface IFetchLineWorkOrdersRepository
    {
        Task<IEnumerable<WorkOrderDto>> FetchWorkOrdersByLineAsync(int lineID);
    }
}
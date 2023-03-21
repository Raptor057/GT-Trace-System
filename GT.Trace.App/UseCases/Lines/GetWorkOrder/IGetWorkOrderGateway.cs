using GT.Trace.Common;

namespace GT.Trace.App.UseCases.Lines.GetWorkOrder
{
    public interface IGetWorkOrderGateway
    {
        Task<Result<WorkOrderDto>> GetWorkOrderAsync(string lineCode);
    }
}
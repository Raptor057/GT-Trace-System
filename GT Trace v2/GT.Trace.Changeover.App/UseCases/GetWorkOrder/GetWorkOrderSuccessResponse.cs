using GT.Trace.Changeover.App.Dtos;

namespace GT.Trace.Changeover.App.UseCases.GetWorkOrder
{
    public sealed record GetWorkOrderSuccessResponse(WorkOrderDto WorkOrder) : GetWorkOrderResponse;
}
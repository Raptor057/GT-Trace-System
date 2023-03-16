using GT.Trace.Changeover.App.Dtos;
using GT.Trace.Changeover.App.Gateways;
using GT.Trace.Changeover.Infra.Daos;

namespace GT.Trace.Changeover.Infra.Gateways
{
    internal class SqlWorkOrderGateway : IWorkOrderGateway
    {
        private readonly WorkOrderDao _workOrders;

        public SqlWorkOrderGateway(WorkOrderDao workOrders)
        {
            _workOrders = workOrders;
        }

        public async Task<WorkOrderDto?> GetLineWorkOrderAsync(int lineID)
        {
            var workOrder = await _workOrders.GetActiveWorkOrderByLine(lineID).ConfigureAwait(false);
            if (workOrder == null)
            {
                return null;
            }

            return new WorkOrderDto(workOrder.id_line, workOrder.codew, workOrder.part_number.Trim(), workOrder.rev.Trim());
        }
    }
}
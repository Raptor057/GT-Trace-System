using GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID;
using GT.Trace.Infra.Daos;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Infra.Gateways
{
    internal class SqlWorkOrderGateway : IWorkOrderGateway
    {
        private readonly WorkOrderDao _workOrders;

        private readonly ILogger<SqlWorkOrderGateway> _logger;

        public SqlWorkOrderGateway(WorkOrderDao workOrders, ILogger<SqlWorkOrderGateway> logger)
        {
            _workOrders = workOrders;
            _logger = logger;
        }

        public async Task IncreaseWorkOrderQuantityAsync(int lineID, string workOrderCode, int amount)
        {
            _logger.LogInformation($"SqlWorkOrderGateway::IncreaseWorkOrderQuantityAsync(lineID = {lineID}, workOrderCode = \"{workOrderCode}\", amount = {amount})");
            var wo = await _workOrders.GetWorkOrderByCodeAsync(workOrderCode).ConfigureAwait(false);
            await _workOrders.UpdateWorkOrderQuantity(lineID, workOrderCode, amount + (wo.current_qty ?? 0)).ConfigureAwait(false);
        }
    }
}
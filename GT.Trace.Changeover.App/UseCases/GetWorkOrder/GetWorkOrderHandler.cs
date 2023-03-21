using GT.Trace.Changeover.App.Gateways;
using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Changeover.App.UseCases.GetWorkOrder
{
    internal sealed class GetWorkOrderHandler : IInteractor<GetWorkOrderByLineIDRequest, GetWorkOrderResponse>
    {
        private readonly ILogger<GetWorkOrderHandler> _logger;

        private readonly IWorkOrderGateway _workOrders;

        public GetWorkOrderHandler(ILogger<GetWorkOrderHandler> logger, IWorkOrderGateway workOrders)
        {
            _logger = logger;
            _workOrders = workOrders;
        }

        public async Task<GetWorkOrderResponse> Handle(GetWorkOrderByLineIDRequest request, CancellationToken cancellationToken)
        {
            var workOrder = await _workOrders.GetLineWorkOrderAsync(request.LineID).ConfigureAwait(false);
            if (workOrder == null)
            {
                return new GetWorkOrderFailureResponse($"No se encontró orden de fabricación activa para la línea #{request.LineID}.");
            }

            return new GetWorkOrderSuccessResponse(workOrder);
        }
    }
}
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetWorkOrder
{
    internal sealed class GetWorkOrderHandler
        : ResultInteractorBase<GetWorkOrderRequest, GetWorkOrderResponse>
    {
        private readonly IGetWorkOrderGateway _gateway;

        public GetWorkOrderHandler(IGetWorkOrderGateway gateway)
        {
            _gateway = gateway;
        }

        public override async Task<Result<GetWorkOrderResponse>> Handle(GetWorkOrderRequest request, CancellationToken cancellationToken)
        {
            var getWorkOrderResult = await _gateway.GetWorkOrderAsync(request.LineCode).ConfigureAwait(false);
            Console.WriteLine("test");
            if (getWorkOrderResult is ISuccess<WorkOrderDto> success)
            {
                return OK(new GetWorkOrderResponse(success.Data));
            }
            return Fail((getWorkOrderResult as IFailure)!.Message);
        }
    }
}
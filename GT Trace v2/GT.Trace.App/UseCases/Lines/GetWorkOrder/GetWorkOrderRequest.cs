using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetWorkOrder
{
    public sealed record GetWorkOrderRequest(string LineCode) : IResultRequest<GetWorkOrderResponse>;
}
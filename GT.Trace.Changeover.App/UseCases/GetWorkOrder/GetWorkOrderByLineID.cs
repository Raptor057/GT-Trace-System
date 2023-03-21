using GT.Trace.Common.CleanArch;

namespace GT.Trace.Changeover.App.UseCases.GetWorkOrder
{
    public sealed record GetWorkOrderByLineIDRequest(int LineID) : IRequest<GetWorkOrderResponse>;
}
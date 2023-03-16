using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.MaterialLoading.FetchLineWorkOrders
{
    public record FetchLineWorkOrdersRequest(int LineID) : IResultRequest<FetchLineWorkOrdersResponse>;
}
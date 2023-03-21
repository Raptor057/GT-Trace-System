using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.MaterialLoading.FetchLineWorkOrders
{
    internal sealed class FetchLineWorkOrdersHandler : ResultInteractorBase<FetchLineWorkOrdersRequest, FetchLineWorkOrdersResponse>
    {
        private readonly IFetchLineWorkOrdersRepository _repository;

        public FetchLineWorkOrdersHandler(IFetchLineWorkOrdersRepository repository)
        {
            _repository = repository;
        }

        public override async Task<Result<FetchLineWorkOrdersResponse>> Handle(FetchLineWorkOrdersRequest request, CancellationToken cancellationToken)
        {
            var workOrders = await _repository.FetchWorkOrdersByLineAsync(request.LineID).ConfigureAwait(false);
            return OK(new FetchLineWorkOrdersResponse(workOrders));
        }
    }
}
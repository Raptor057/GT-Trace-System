using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.MaterialLoading.FetchLines
{
    internal sealed class FetchLinesHandler : ResultInteractorBase<FetchLinesRequest, FetchLinesResponse>
    {
        private readonly IFetchLinesRepository _repository;

        public FetchLinesHandler(IFetchLinesRepository repository)
        {
            _repository = repository;
        }

        public override async Task<Result<FetchLinesResponse>> Handle(FetchLinesRequest request, CancellationToken cancellationToken)
        {
            var lines = await _repository.FetchLinesAsync().ConfigureAwait(false);
            return OK(new FetchLinesResponse(lines));
        }
    }
}
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseLines
{
    internal sealed class FetchPointOfUseLinesHandler : ResultInteractorBase<FetchPointOfUseLinesRequest, FetchPointOfUseLinesResponse>
    {
        private readonly IFetchPointOfUseLinesRepository _repo;

        public FetchPointOfUseLinesHandler(IFetchPointOfUseLinesRepository repo)
        {
            _repo = repo;
        }

        public override async Task<Result<FetchPointOfUseLinesResponse>> Handle(FetchPointOfUseLinesRequest request, CancellationToken cancellationToken)
        {
            var lineCodes = await _repo.FetchPointOfUseLinesAsync(request.PointOfUseCode).ConfigureAwait(false);
            return OK(new FetchPointOfUseLinesResponse(lineCodes));
        }
    }
}
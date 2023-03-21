using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis
{
    internal sealed class FetchPointOfUseEtisHandler : ResultInteractorBase<FetchPointOfUseEtisRequest, FetchPointOfUseEtisResponse>
    {
        private readonly IFetchPointOfUseEtisRepository _repository;

        public FetchPointOfUseEtisHandler(IFetchPointOfUseEtisRepository repository)
        {
            _repository = repository;
        }

        public override async Task<Result<FetchPointOfUseEtisResponse>> Handle(FetchPointOfUseEtisRequest request, CancellationToken cancellationToken)
        {
            var etis = await _repository.FetchPointOfUseEtisAsync(request.PointOfUseCode).ConfigureAwait(false);
            return OK(new FetchPointOfUseEtisResponse(etis));
        }
    }
}
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Etis.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Etis.App.UseCases.GetEti
{
    internal sealed class GetEtiHandler : ResultInteractorBase<GetEtiRequest, GetEtiResponse>
    {
        private readonly IEtiRepository _etis;

        private readonly ILogger<GetEtiHandler> _logger;

        public GetEtiHandler(IEtiRepository etis, ILogger<GetEtiHandler> logger)
        {
            _etis = etis;
            _logger = logger;
        }

        public override async Task<Result<GetEtiResponse>> Handle(GetEtiRequest request, CancellationToken cancellationToken)
        {
            var eti = await _etis.TryGetEtiByIDAsync(request.EtiID, request.EtiNo).ConfigureAwait(false);
            if (eti == null)
            {
                return Fail($"No se encontró información sobre la ETI#{request.EtiID} \"{request.EtiNo}\".");
            }

            return OK(new GetEtiResponse(eti.Id, eti.Number, eti.ComponentNo, eti.Revision, eti.LotNo, eti.IsEnabled));
        }
    }
}
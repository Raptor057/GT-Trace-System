using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Etis.Domain.Repositories;
using GT.Trace.Etis.Domain.Services;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Etis.App.UseCases.GetEtiInfo
{
    internal sealed class GetEtiInfoHandler : ResultInteractorBase<GetEtiInfoRequest, GetEtiInfoResponse>
    {
        private readonly IEtiParserService _etiParser;

        private readonly IEtiRepository _etis;

        private readonly ILogger<GetEtiInfoHandler> _logger;

        public GetEtiInfoHandler(IEtiRepository etis, IEtiParserService etiParser, ILogger<GetEtiInfoHandler> logger)
        {
            _etis = etis;
            _etiParser = etiParser;
            _logger = logger;
        }

        public override async Task<Result<GetEtiInfoResponse>> Handle(GetEtiInfoRequest request, CancellationToken cancellationToken)
        {
            if (!_etiParser.TryParseEti(request.ScannerInput!, out var etiId, out var etiNo))
            {
                return Fail($"Ocurrió un problema al procesar el escaneo \"{request.ScannerInput}\".");
            }

            var eti = await _etis.TryGetEtiByIDAsync(etiId, etiNo).ConfigureAwait(false);
            if (eti == null)
            {
                return Fail($"Ocurrió un problema al procesar el escaneo \"{request.ScannerInput}\".");
            }

            return OK(new GetEtiInfoResponse(eti.Id, eti.Number, eti.ComponentNo, eti.Revision, eti.LotNo, eti.IsEnabled));
        }
    }
}
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Domain.Repositories;
using GT.Trace.Domain.Services;
using Microsoft.Extensions.Logging;

namespace GT.Trace.App.UseCases.MaterialLoading.FetchEtiPointsOfUse
{
    internal sealed class FetchEtiPointsOfUseHandler : ResultInteractorBase<FetchEtiPointsOfUseRequest, FetchEtiPointsOfUseResponse>
    {
        private readonly ILabelParserService _labelParser;

        private readonly IEtiRepository _etis;

        private readonly IBomRepository _bom;

        private readonly ILogger<FetchEtiPointsOfUseHandler> _logger;

        public FetchEtiPointsOfUseHandler(ILabelParserService labelParser, IEtiRepository etis, IBomRepository bom, ILogger<FetchEtiPointsOfUseHandler> logger)
        {
            _bom = bom;
            _etis = etis;
            _logger = logger;
            _labelParser = labelParser;
        }

        public override async Task<Result<FetchEtiPointsOfUseResponse>> Handle(FetchEtiPointsOfUseRequest request, CancellationToken cancellationToken)
        {
            if (!_labelParser.TryParseEti(request.EtiNo, out var etiID, out var etiNo))
            {
                return Fail($"Ocurrió un problema al procesar el número de ETI \"{request.EtiNo}\".");
            }

            if (etiID == null || etiID < 0 || string.IsNullOrWhiteSpace(etiNo))
            {
                return Fail($"ID [ {etiID} ] o número [ {etiNo} ] de ETI no válido.");
            }

            var eti = await _etis.TryGetEtiByIDAsync(etiID.Value, etiNo).ConfigureAwait(false);

            var bom = await _bom.FetchBomAsync(request.PartNo, request.LineCode).ConfigureAwait(false);

            var pointsOfUse =
                bom
                    .Where(item => string.Compare(item.CompNo, eti.Component.Number, true) == 0)
                    .Select(item => item.PointOfUseCode)
                    .ToArray();

            return OK(new FetchEtiPointsOfUseResponse(pointsOfUse));
        }

    }
}
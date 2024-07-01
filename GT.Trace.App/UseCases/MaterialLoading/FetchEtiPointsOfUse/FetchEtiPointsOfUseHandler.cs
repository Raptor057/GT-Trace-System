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

        //public override async Task<Result<FetchEtiPointsOfUseResponse>> Handle(FetchEtiPointsOfUseRequest request, CancellationToken cancellationToken)
        //{
        //    if (!_labelParser.TryParseEti(request.EtiNo, out var etiID, out var etiNo))
        //    {
        //        return Fail($"Ocurrió un problema al procesar el número de ETI \"{request.EtiNo}\".");
        //    }

        //    if (etiID == null || etiID < 0 || string.IsNullOrWhiteSpace(etiNo))
        //    {
        //        return Fail($"ID [ {etiID} ] o número [ {etiNo} ] de ETI no válido.");
        //    }

        //    var eti = await _etis.TryGetEtiByIDAsync(etiID.Value, etiNo).ConfigureAwait(false);

        //    var bom = await _bom.FetchBomAsync(request.PartNo, request.LineCode).ConfigureAwait(false);

        //    var pointsOfUse =
        //        bom
        //            .Where(item => string.Compare(item.CompNo, eti.Component.Number, true) == 0)
        //            .Select(item => item.PointOfUseCode)
        //            .ToArray();

        //    return OK(new FetchEtiPointsOfUseResponse(pointsOfUse));
        //}
        public override async Task<Result<FetchEtiPointsOfUseResponse>> Handle(FetchEtiPointsOfUseRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!_labelParser.TryParseEti(request.EtiNo, out var etiID, out var etiNo))
                {
                    return Fail($"Ocurrió un problema al procesar el número de ETI \"{request.EtiNo}\".");
                }

                if (etiID == null || etiID < 0 || string.IsNullOrWhiteSpace(etiNo))
                {
                    return Fail($"ID [ {etiID} ] o número [ {etiNo} ] de ETI no válido.");
                }

                _logger.LogDebug("ETI ID: {EtiID}, ETI Number: {EtiNo}", etiID, etiNo);

                var eti = await _etis.TryGetEtiByIDAsync(etiID.Value, etiNo).ConfigureAwait(false);

                if (eti?.Component?.Number == null)
                {
                    return Fail("El componente o el número del componente del ETI es nulo.");
                }

                var bom = await _bom.FetchBomAsync(request.PartNo, request.LineCode).ConfigureAwait(false);

                _logger.LogDebug("Fetched BOM items count: {Count}", bom.Count());

                var pointsOfUse =
                    bom
                        .Where(item => string.Compare(item.CompNo.Trim(), eti.Component.Number.Trim(), true) == 0)
                        .Select(item => item.PointOfUseCode.Trim())
                        .ToArray();

                return OK(new FetchEtiPointsOfUseResponse(pointsOfUse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching ETI points of use.");
                return Fail("Ocurrió un error al obtener los puntos de uso del ETI.");
            }
        }


    }
}
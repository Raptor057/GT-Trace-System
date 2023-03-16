using GT.Trace.App.Services;
using GT.Trace.App.UseCases.Lines.GetLine;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID
{
    internal sealed class GetNewSubAssemblyIDHandler : ResultInteractorBase<GetNewSubAssemblyIDRequest, GetNewSubAssemblyIDResponse>
    {
        private readonly IGetNewSubAssemblyIDRepository _getNewSubAssmblyID;

        private readonly IGetLineRepository _getLine;

        private readonly ICegidRadioService _radio;

        private readonly IBomService _bom;

        private readonly IPointOfUseService _pointsOfUse;

        private readonly IWorkOrderGateway _workOrders;

        public GetNewSubAssemblyIDHandler(IGetNewSubAssemblyIDRepository getNewSubAssmblyID, IGetLineRepository getLine, ICegidRadioService radio, IBomService bom, IPointOfUseService pointsOfUse, IWorkOrderGateway workOrders)
        {
            _getNewSubAssmblyID = getNewSubAssmblyID;
            _getLine = getLine;
            _radio = radio;
            _bom = bom;
            _pointsOfUse = pointsOfUse;
            _workOrders = workOrders;
        }

        public override async Task<Result<GetNewSubAssemblyIDResponse>> Handle(GetNewSubAssemblyIDRequest request, CancellationToken cancellationToken)
        {
            var line = await _getLine.GetLineByCodeAsync(
                request.LineCode)
                .ConfigureAwait(false);

            if (!line.OutputIsSubAssembly)
            {
                return Fail($"La línea {line.Code} no genera sub ensambles.");
            }

            var missingComponentsCount = line.PointsOfUse.Count(pou => string.IsNullOrWhiteSpace(pou.ActiveEti?.Number ?? ""));

            if (missingComponentsCount > 0)
            {
                return Fail($"La trazabilidad de la línea {line.Code} está incompleta.");
            }

            var etiID = await _getNewSubAssmblyID.ExecuteAsync(
                request.LineCode, line.WorkOrder.PartNo, line.WorkOrder.Revision, line.WorkOrder.Code, line.WorkOrder.StdPackSize)
                .ConfigureAwait(false);

            var errorMessage = await _radio.GenerateFabricationControlFileAsync(
                line.WorkOrder.PartNo, line.WorkOrder.Revision, line.WorkOrder.Code, line.WorkOrder.StdPackSize, etiID)
                .ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                return Fail(errorMessage);
            }

            await _workOrders.IncreaseWorkOrderQuantityAsync(line.ID, line.WorkOrder.Code, line.WorkOrder.StdPackSize).ConfigureAwait(false);

            var bomEntry = await _bom.GetBomEntryForComponentInLine(line.ActivePart.Number, line.Code)
                .ConfigureAwait(false);

            var targetLineCode = "";
            if (bomEntry != null)
            {
                var nextLine = await _getLine.GetLineByCodeAsync(bomEntry.LineCode)
                    .ConfigureAwait(false);

                await _pointsOfUse.LoadMaterialAsync(bomEntry.LineCode, $"SA{etiID:00000000}", bomEntry.PointOfUseCode)
                    .ConfigureAwait(false);

                targetLineCode = nextLine.Code;
            }

            return OK(new GetNewSubAssemblyIDResponse(request.LineCode, etiID, line.WorkOrder.PartNo, line.WorkOrder.Revision, line.ActivePart.Description, line.WorkOrder.Code, line.WorkOrder.StdPackSize, targetLineCode));
        }
    }
}
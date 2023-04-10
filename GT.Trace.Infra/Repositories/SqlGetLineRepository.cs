using GT.Trace.App.UseCases.Lines.GetLine;
using GT.Trace.App.UseCases.Lines.GetLine.Dtos;
using GT.Trace.Infra.Daos;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Repositories
{
    internal class SqlGetLineRepository : IGetLineRepository
    {
        private readonly PointOfUseDao _pointsOfUse;

        private readonly LineDao _lines;

        private readonly WorkOrderDao _workOrders;

        private readonly BomDao _bom;

        public SqlGetLineRepository(LineDao lines, WorkOrderDao workOrders, BomDao bom, PointOfUseDao pointsOfUse)
        {
            _lines = lines;
            _workOrders = workOrders;
            _bom = bom;
            _pointsOfUse = pointsOfUse;
        }

        public async Task<LineDto> GetLineByCodeAsync(string lineCode)
        {
            var prod_unit = await _lines.GetLineByLineCodeAsync(lineCode).ConfigureAwait(false) ?? throw new InvalidOperationException($"Línea {lineCode} no encontrada.");

            var activeWorkOrder = await _workOrders.GetActiveWorkOrderAsync(lineCode).ConfigureAwait(false);

            pro_production production;
            if (string.IsNullOrWhiteSpace(activeWorkOrder))
            {
                production = await _workOrders.GetWorkOrderRunningByLineAsync(prod_unit.id).ConfigureAwait(false) ?? new pro_production();
            }
            else
            {
                production = await _workOrders.GetWorkOrderByCodeAsync(activeWorkOrder).ConfigureAwait(false) ?? new pro_production();
                prod_unit.modelo = production.part_number.Trim();
                prod_unit.active_revision = production.rev;
                prod_unit.codew = production.codew;
            }
            var bom = await _bom.FetchBomAsync(prod_unit.modelo, prod_unit.letter).ConfigureAwait(false);
            var set = await _pointsOfUse.FindActiveEtisAsync(prod_unit.letter, prod_unit.modelo, prod_unit.codew).ConfigureAwait(false);
            var loadedEtis = await _pointsOfUse.FetchAllLoadedEtisAsync(prod_unit.letter).ConfigureAwait(false);

            return new LineDto(
                prod_unit.id,
                prod_unit.letter,
                prod_unit.comments,
                prod_unit.codew,
                new PartDto(prod_unit.modelo, prod_unit.active_revision, ""),
                new WorkOrderDto(production.codew, production.client_name, production.target_qty ?? 0, production.current_qty ?? 0, production.std_pack ?? 0, production.part_number, production.rev),
                bom.Select(item => new PointOfUseDto(
                    item.PointOfUse,
                    new PartDto(item.CompNo, item.CompRev, item.CompDesc),
                    item.ReqQty,
                    int.TryParse(item.Capacity, out var capacity) ? capacity : -1,
                    loadedEtis.Count(eti => eti.PointOfUseCode == item.PointOfUse && eti.ComponentNo == item.CompNo),
                    TryBuildEti(set.FirstOrDefault(eti => eti.ComponentNo == item.CompNo && eti.PointOfUseCode == item.PointOfUse)))),
                prod_unit.output_is_subassembly);
        }

        public async Task<LineDto> GetLineTraceByCodeAsync(string lineCode)
        {
            var prod_unit = await _lines.GetLineByLineCodeAsync(lineCode).ConfigureAwait(false) ?? throw new InvalidOperationException($"Línea {lineCode} no encontrada.");

            var activeWorkOrder = await _workOrders.GetActiveWorkOrderAsync(lineCode).ConfigureAwait(false);

            pro_production production;
            if (string.IsNullOrWhiteSpace(activeWorkOrder))
            {
                production = await _workOrders.GetWorkOrderRunningByLineAsync(prod_unit.id).ConfigureAwait(false) ?? new pro_production();
            }
            else
            {
                production = await _workOrders.GetWorkOrderByCodeAsync(activeWorkOrder).ConfigureAwait(false) ?? new pro_production();
                prod_unit.modelo = production.part_number.Trim();
                prod_unit.active_revision = production.rev;
                prod_unit.codew = production.codew;
            }
            var bom = await _bom.FetchBomAsync(prod_unit.modelo, prod_unit.letter).ConfigureAwait(false);
            var set = await _pointsOfUse.FindActiveTraceEtisAsync(prod_unit.letter, prod_unit.modelo, prod_unit.codew).ConfigureAwait(false);
            var loadedEtis = await _pointsOfUse.FetchAllLoadedEtisAsync(prod_unit.letter).ConfigureAwait(false);

            return new LineDto(
                prod_unit.id,
                prod_unit.letter,
                prod_unit.comments,
                prod_unit.codew,
                new PartDto(prod_unit.modelo, prod_unit.active_revision, ""),
                new WorkOrderDto(production.codew, production.client_name, production.target_qty ?? 0, production.current_qty ?? 0, production.std_pack ?? 0, production.part_number, production.rev),
                bom.Select(item => new PointOfUseDto(
                    item.PointOfUse,
                    new PartDto(item.CompNo, item.CompRev, item.CompDesc),
                    item.ReqQty,
                    int.TryParse(item.Capacity, out var capacity) ? capacity : -1,
                    loadedEtis.Count(eti => eti.PointOfUseCode == item.PointOfUse && eti.ComponentNo == item.CompNo),
                    TryBuildEti(set.FirstOrDefault(eti => eti.ComponentNo == item.CompNo && eti.PointOfUseCode == item.PointOfUse)))),
                prod_unit.output_is_subassembly);
        }

        private EtiDto? TryBuildEti(PointOfUseEtis? poue)
        {
            EtiDto? eti = null;
            if (poue != null)
            {
                eti = new EtiDto(poue.EtiNo, poue.Size, poue.PackingCount);
            }
            return eti;
        }
    }
}
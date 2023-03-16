using GT.Trace.Domain.Entities;
using GT.Trace.Domain.Events;
using GT.Trace.Domain.Repositories;
using GT.Trace.Infra.Daos;
using System.Transactions;

namespace GT.Trace.Infra.Repositories
{
    internal class SqlLineRepository : ILineRepository
    {
        private readonly PointOfUseDao _pointsOfUse;

        private readonly LineDao _lines;

        private readonly WorkOrderDao _workOrders;

        private readonly BomDao _bom;

        public SqlLineRepository(LineDao lines, WorkOrderDao workOrders, BomDao bom, PointOfUseDao pointsOfUse)
        {
            _lines = lines;
            _workOrders = workOrders;
            _bom = bom;
            _pointsOfUse = pointsOfUse;
        }

        public async Task<Line> GetByCodeAsync(string lineCode, string? workOrderCode = null)
        {
            workOrderCode = await _workOrders.GetActiveWorkOrderAsync(lineCode).ConfigureAwait(false);

            var prod_unit = await _lines.GetLineByLineCodeAsync(lineCode).ConfigureAwait(false);

            Entities.pro_production production;
            if (string.IsNullOrWhiteSpace(workOrderCode))
            {
                production = await _workOrders.GetWorkOrderByCodeAsync(prod_unit.id, prod_unit.codew).ConfigureAwait(false);
            }
            else
            {
                production = await _workOrders.GetWorkOrderByCodeAsync(workOrderCode).ConfigureAwait(false);
                prod_unit.modelo = production.part_number.Trim();
                prod_unit.active_revision = production.rev;
                prod_unit.codew = production.codew;
            }
            production.part_number = production.part_number.Trim();
            production.client_name = production.client_name.Trim();
            production.ratio = production.ratio.Trim();
            production.rev = production.rev.Trim();
            production.ref_ext = production.ref_ext.Trim();

            var bom = await _bom.FetchBomAsync(production.part_number, prod_unit.letter).ConfigureAwait(false);
            var set = await _pointsOfUse.FindActiveEtisAsync(prod_unit.letter, production.part_number, production.codew).ConfigureAwait(false);
            var loadedEtis = (await _pointsOfUse.FetchAllLoadedEtisAsync(prod_unit.letter).ConfigureAwait(false)).Where(item => string.Compare(item.PartNo, production.part_number, true) == 0);

            var loadedEtisByPointOfUse = loadedEtis.Select(eti => eti.PointOfUseCode).Distinct()
                    .ToDictionary(pou => pou, pou => loadedEtis.Where(x => x.PointOfUseCode == pou)
                        .Select(eti => eti.ComponentNo).Distinct()
                            .ToDictionary(comp => comp, comp => loadedEtis.Where(x => x.PointOfUseCode == pou && x.ComponentNo == comp).Select(x => x.EtiNo).ToList()));

            var activePart = string.IsNullOrWhiteSpace(prod_unit.modelo)
                    ? null
                    : Part.Create(prod_unit.modelo, Revision.New(prod_unit.active_revision));

            var lineBom = bom.Select(item => new BomComponent(item.PointOfUse, item.CompNo, item.CompRev, item.CompDesc, int.Parse(item.Capacity)));

            var lineSet = set.Select(item => new SetComponent(item.PointOfUseCode, item.ComponentNo, item.EtiNo));

            var lineWorkOrder = new WorkOrder(production.id, production.codew, Part.Create(production.part_number, Revision.New(production.rev)), production.ref_ext, production.order, production.line);

            return new Line(
                prod_unit.id,
                prod_unit.letter,
                prod_unit.comments,
                lineWorkOrder,
                activePart,
                lineBom,
                lineSet,
                loadedEtisByPointOfUse,
                prod_unit.output_is_subassembly);
        }

        public async Task SaveAsync(Line line)
        {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            foreach (var e in line.GetEvents())
            {
                if (e is MaterialLoadedEvent materialLoaded)
                    await OnAsync(materialLoaded).ConfigureAwait(false);
                else if (e is MaterialUnloadedEvent materialUnloaded)
                    await OnAsync(materialUnloaded).ConfigureAwait(false);
                else if (e is MaterialUsedEvent materialUsed)
                    await OnAsync(materialUsed).ConfigureAwait(false);
                else if (e is MaterialReturnedEvent materialReturned)
                    await OnAsync(materialReturned).ConfigureAwait(false);
            }
            tx.Complete();
        }

        private async Task OnAsync(MaterialLoadedEvent e)
        {
            await _pointsOfUse.LoadEtiAsync(e.PartNo, e.WorkOrderCode, e.EtiNo, e.PointOfUseCode, e.ComponentNo).ConfigureAwait(false);
            //await _traza.SaveEtiInPointOfUse(e.LineCode, e.WorkOrderCode, e.PartNo, e.Folio, e.OrderLine, e.Order, e.EtiNo, e.PointOfUseCode, e.OperatorNo, e.LotNo, e.ComponentNo, e.Comments);
        }

        private async Task OnAsync(MaterialUnloadedEvent e)
        {
            await _pointsOfUse.UnloadEtiAsync(e.PartNo, e.WorkOrderCode, e.EtiNo).ConfigureAwait(false);
            //await _traza.UnloadMaterialAsync(e.EtiNo, e.ComponentNo, e.LineCode, e.UtcTimeStamp).ConfigureAwait(false);
        }

        private async Task OnAsync(MaterialUsedEvent e)
        {
            await _pointsOfUse.UseEtiAsync(e.PartNo, e.WorkOrderCode, e.EtiNo, e.PointOfUseCode).ConfigureAwait(false);
        }

        private async Task OnAsync(MaterialReturnedEvent e)
        {
            await _pointsOfUse.ReturnEtiAsync(e.PartNo, e.WorkOrderCode, e.EtiNo, e.IsDepleted).ConfigureAwait(false);
        }
    }
}
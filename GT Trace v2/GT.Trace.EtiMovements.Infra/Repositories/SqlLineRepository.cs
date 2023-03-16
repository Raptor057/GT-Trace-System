using GT.Trace.Common;
using GT.Trace.EtiMovements.App.Services;
using GT.Trace.EtiMovements.Domain.Entities;
using GT.Trace.EtiMovements.Domain.Events;
using GT.Trace.EtiMovements.Domain.Repositories;
using GT.Trace.EtiMovements.Infra.Daos;
using GT.Trace.EtiMovements.Infra.Entities;
using System.Transactions;

namespace GT.Trace.EtiMovements.Infra.Repositories
{
    internal class SqlLineRepository
        : ILineRepository
    {
        private readonly ILineServices _lines;

        private readonly PointOfUseEtiDao _movements;

        public SqlLineRepository(ILineServices lines, PointOfUseEtiDao movements)
        {
            _lines = lines;
            _movements = movements;
        }

        public async Task<Result<Line>> GetAsync(string lineCode)
        {
            var workOrderResult = await _lines.GetWorkOrderAsync(lineCode).ConfigureAwait(false);
            if (workOrderResult is IFailure workOrderFailure)
            {
                return Result.Fail<Line>(workOrderFailure.Message);
            }
            var workOrderData = (workOrderResult as ISuccess<App.Dtos.WorkOrderDto>)?.Data ?? throw new NullReferenceException();

            var workOrder = new WorkOrder(workOrderData.PartNo, workOrderData.Revision);

            var getBomResult = await _lines.GetBomAsync(workOrder.PartNo, lineCode).ConfigureAwait(false);
            if (getBomResult is IFailure getBomFailure)
            {
                return Result.Fail<Line>(getBomFailure.Message);
            }

            var bomItems = (getBomResult as ISuccess<IEnumerable<App.Dtos.BomComponentDto>>)?.Data ?? throw new NullReferenceException();
            var pointsOfUse = bomItems.Select(item => item.PointOfUseCode).Distinct();

            var loadedEtis = await _movements.GetLoadedEtis(pointsOfUse).ConfigureAwait(false);
            var activeEtis = await _movements.GetActiveEtis(pointsOfUse).ConfigureAwait(false);

            var bom = bomItems.Select(
                item => new PointOfUse(
                    item.PointOfUseCode,
                    item.ComponentNo,
                    Revision.New(item.CompRev),
                    item.Capacity,
                    loadedEtis.Where(eti => eti.ComponentNo == item.ComponentNo && eti.PointOfUseCode == item.PointOfUseCode).Select(eti => eti.EtiNo),
                    // WARNING! This will cause an exception if there are more than 1 active ETI in a point of use.
                    activeEtis.SingleOrDefault(eti => eti.ComponentNo == item.ComponentNo && eti.PointOfUseCode == item.PointOfUseCode)?.EtiNo));

            return Result.OK(new Line(lineCode, workOrder, bom));
        }

        public async Task SaveAsync(Line line)
        {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            foreach (var e in line.GetEvents())
            {
                if (e is EtiLoadedEvent etiLoaded)
                {
                    await OnAsync(etiLoaded).ConfigureAwait(false);
                }
                else if (e is EtiUnloadedEvent etiUnloaded)
                {
                    await OnAsync(etiUnloaded).ConfigureAwait(false);
                }
                else if (e is EtiUsedEvent etiUsed)
                {
                    await OnAsync(etiUsed).ConfigureAwait(false);
                }
                else if (e is EtiReturnedEvent etiReturned)
                {
                    await OnAsync(etiReturned).ConfigureAwait(false);
                }
            }
            tx.Complete();
        }

        private async Task OnAsync(EtiLoadedEvent e)
        {
            var movement = new PointOfUseEtis
            {
                PointOfUseCode = e.PointOfUseCode,
                EtiNo = e.EtiNo,
                ComponentNo = e.ComponentNo,
                UtcEffectiveTime = e.StartTime.ToUniversalTime(),
                LotNo = e.LotNo
            };
            await _movements.AddAsync(movement).ConfigureAwait(false);
        }

        private async Task OnAsync(EtiUnloadedEvent e)
        {
            var movement = await _movements.GetEtiLastMovementAsync(e.EtiNo).ConfigureAwait(false)
                ?? throw new NullReferenceException($"Ultimo movimiento de la ETI {e.EtiNo} no encontrado.");
            movement.UtcExpirationTime = e.EndTime.ToUniversalTime();
            await _movements.UpdateAsync(movement).ConfigureAwait(false);
        }

        private async Task OnAsync(EtiUsedEvent e)
        {
            var movement = await _movements.GetEtiLastMovementAsync(e.EtiNo).ConfigureAwait(false)
                ?? throw new NullReferenceException($"Ultimo movimiento de la ETI {e.EtiNo} no encontrado.");
            movement.UtcUsageTime = e.UsageTime.ToUniversalTime();

            // We can use the value stored in e.EtiToReplace to return and deplete the previously used ETI.
            var active = await _movements.GetLastUsedNotReturnedEtiAsync(movement.PointOfUseCode, movement.ComponentNo).ConfigureAwait(false);
            if (active != null)
            {
                active.IsDepleted = true;
                active.UtcExpirationTime = DateTime.Now.ToUniversalTime();
                await _movements.UpdateAsync(active).ConfigureAwait(false);
            }

            await _movements.UpdateAsync(movement).ConfigureAwait(false);
        }

        private async Task OnAsync(EtiReturnedEvent e)
        {
            var movement = await _movements.GetEtiLastMovementAsync(e.EtiNo).ConfigureAwait(false)
                ?? throw new NullReferenceException($"Ultimo movimiento de la ETI {e.EtiNo} no encontrado.");

            movement.UtcExpirationTime = e.EndTime.ToUniversalTime();
            await _movements.UpdateAsync(movement).ConfigureAwait(false);
        }
    }
}
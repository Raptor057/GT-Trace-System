using GT.Trace.App.Services;
using GT.Trace.Common;
using GT.Trace.Domain.PointsOfUse.Entities;
using GT.Trace.Domain.PointsOfUse.Events;
using GT.Trace.Domain.PointsOfUse.Repositories;
using GT.Trace.Infra.Daos;
using System.Transactions;

namespace GT.Trace.Infra.Repositories
{
    internal class SqlPointOfUseEtiRepository : IPointOfUseEtiRepository
    {
        private readonly IEtiService _etis;

        private readonly GttDao _gtt;

        public SqlPointOfUseEtiRepository(IEtiService etis, GttDao gtt)
        {
            _etis = etis;
            _gtt = gtt;
        }

        public async Task<Result<PointOfUseEti>> GetAsync(string etiNo, string partNo, string revision, string pointOfUseCode)
        {
            var response = await _etis.GetEtiInfoAsync(etiNo).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                return Result.Fail<PointOfUseEti>(response.Message);
            }

            var componentNo = response!.Data!.ComponentNo;

            var bomItems = await _gtt.GetBomItems(partNo, revision);

            var bom = new Bom(partNo, revision, bomItems.Select(item => new BomItem(item.ComponentNo, item.PointOfUseCode, item.Capacity)).ToList());

            if (!bom.HasPointOfUse(pointOfUseCode))
            {
                return Result.Fail<PointOfUseEti>($"El túnel {pointOfUseCode} no es parte del BOM de {partNo} {revision}.");
            }

            if (!bom.TryGetComponent(componentNo!, pointOfUseCode, out var component) || component == null)
            {
                return Result.Fail<PointOfUseEti>($"El componente {componentNo} no es parte del BOM de {partNo} {revision} o no corresponde con el túnel {pointOfUseCode}.");
            }

            var loadState = await _gtt.LoadBomComponentStateOrNew(pointOfUseCode, componentNo!).ConfigureAwait(false);

            if (component.Capacity <= loadState.LoadSize)
            {
                return Result.Fail<PointOfUseEti>($"El túnel {pointOfUseCode} el componente {componentNo} no es parte del BOM de {partNo} {revision} o no corrresponde con .");
            }

            var item = await _gtt.GetLastPointOfUseEtiEntry(etiNo).ConfigureAwait(false);

            return Result.OK(item != null
                ? PointOfUseEti.Create(item.PointOfUseCode, item.EtiNo, item.ComponentNo, item.UtcEffectiveTime, item.UtcExpirationTime, item.UtcUsageTime, item.IsDepleted)
                : PointOfUseEti.Create(pointOfUseCode, response.Data.EtiNo, componentNo!, null, null, null, false));
        }

        public async Task SaveAsync(PointOfUseEti eti)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            foreach (var e in eti.GetEvents())
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
            transaction.Complete();
        }

        private async Task OnAsync(EtiLoadedEvent e)
        {
            await _gtt.LoadEtiAsync(e.EtiNo, e.ComponentNo, e.PointOfUseCode, e.EffectiveTime).ConfigureAwait(false);
        }

        private async Task OnAsync(EtiUnloadedEvent e)
        {
            await _gtt.UnloadEtiAsync(e.EtiNo, e.ExpirationTime).ConfigureAwait(false);
        }

        private async Task OnAsync(EtiUsedEvent e)
        {
            await _gtt.UseEtiAsync(e.EtiNo, e.UsageTime).ConfigureAwait(false);
        }

        private async Task OnAsync(EtiReturnedEvent e)
        {
            await _gtt.ReturnEtiAsync(e.EtiNo, e.IsDepleted, e.ExpirationTime).ConfigureAwait(false);
        }
    }
}
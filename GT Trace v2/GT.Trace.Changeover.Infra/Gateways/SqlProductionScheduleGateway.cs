using GT.Trace.Changeover.App.UseCases.ApplyChangeover;
using GT.Trace.Changeover.Infra.Daos;

namespace GT.Trace.Changeover.Infra.Gateways
{
    internal class SqlProductionScheduleGateway : IProductionScheduleGateway
    {
        private readonly Daos.ProductionScheduleDao _productionSchedule;

        public SqlProductionScheduleGateway(ProductionScheduleDao productionSchedule)
        {
            _productionSchedule = productionSchedule;
        }

        public async Task UpdateProductionSchedule(string lineCode, string partNo, string revision, string workOrderCode)
        {
            await _productionSchedule.ExpireProductionSchedule(lineCode).ConfigureAwait(false);
            await _productionSchedule.ActivateProductionSchedule(lineCode, partNo, workOrderCode, revision).ConfigureAwait(false);
            await _productionSchedule.UpdateLineGamma(lineCode, partNo, revision).ConfigureAwait(false);
        }
    }
}
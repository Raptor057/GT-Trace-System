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

        //Agregado para corregir el BUG que no se actualiza la tabla LineProductionSchedule al aplicar cambio de modelo en cualquier linea
        public async Task<bool> FindLineModelCapabilitiesAsync(string lineCode, string partNo)
        {
         var LineModelCapabilities = await _productionSchedule.FindLineModelCapabilities(lineCode, partNo) > 0;
         return LineModelCapabilities;
        }

        public async Task InsertModelCapabilitiesAsync(string lineCode, string partNo)
        {

            await _productionSchedule.InsertModelCapabilities(lineCode, partNo).ConfigureAwait(false);
        }

        public async Task InsertModelCapabilitiesNewAsync(string lineCode, string partNo)
        {

            await _productionSchedule.InsertModelCapabilitiesNew(lineCode, partNo).ConfigureAwait(false);
        }

        public async Task UpdateProductionSchedule(string lineCode, string partNo, string revision, string workOrderCode)
        {
            await _productionSchedule.ExpireProductionSchedule(lineCode).ConfigureAwait(false);
            await _productionSchedule.ActivateProductionSchedule(lineCode, partNo, workOrderCode, revision).ConfigureAwait(false);
            await _productionSchedule.UpdateLineGamma(lineCode, partNo, revision).ConfigureAwait(false);
        }
    }
}
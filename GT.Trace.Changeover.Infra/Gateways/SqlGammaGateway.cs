using GT.Trace.Changeover.App.Dtos;
using GT.Trace.Changeover.App.Gateways;
using GT.Trace.Changeover.Infra.Daos;
using GT.Trace.Changeover.Infra.Daos.Entities;

namespace GT.Trace.Changeover.Infra.Gateways
{
    internal class SqlGammaGateway : IGammaGateway
    {
        private readonly GammaDao _gamma;

        public SqlGammaGateway(GammaDao gamma)
        {
            _gamma = gamma;
        }

        //Se agrego para evitar el cambio de linea si falta la gamma en la base de datos
        //RA: 07/05/2023.
        public async Task<bool> GammaDataAsync(string partNo, string lineCode)
        {
            var BoomCountResultGammaData = await _gamma.GammaDataAsync(partNo, lineCode).ConfigureAwait(false) > 0;
            return BoomCountResultGammaData;
        }

        //Se agrego para evitar el cambio de linea si falta la gamma en la base de datos
        //RA: 07/05/2023.
        public async Task UpdateGamaTrazabAsync(string partNo, string lineCode)
        {
            await _gamma.UpdateGamaTrazabAsync(partNo,lineCode).ConfigureAwait(false);
        }

        public async Task<IEnumerable<GammaItemDto>> GetGammaAsync(string lineCode, string partNo, string revision)
        {

            return (await _gamma.GetGammaAsync(partNo, revision).ConfigureAwait(false))
                .Select(item => new GammaItemDto(item.PointOfUse, item.CompNo, item.CompRev, item.CompRev2, item.CompDesc));
        }

        public async Task<IEnumerable<GammaItemDto>> GetOutgoingComponentsAsync(string ogPartNo, string ogRevision, string icPartNo, string icRevision)
        {
            return (await _gamma.GetComponentDifferences(ogPartNo, ogRevision, icPartNo, icRevision).ConfigureAwait(false))
                .Select(item => new GammaItemDto(item.PointOfUse, item.CompNo, item.CompRev, item.CompRev2, item.CompDesc));
        }

        public async Task<bool> GetEmptyCapacitiesAsync(string partNo, string lineCode)
        {
            return (await _gamma.GetEmptyCapacities(partNo, lineCode).ConfigureAwait(false) > 1);
        }

        public async Task<bool> GetRepeatedComponentsAsync(string partNo, string lineCode)
        {
            var RepeatedComponents = await _gamma.GetRepeatedComponents(partNo, lineCode).ConfigureAwait(false);
            // Contar las filas de repeatedComponents
            int count = RepeatedComponents.Count();
            // Retornar true si hay más de una fila, de lo contrario false
            return count > 1;
        }
    }
}
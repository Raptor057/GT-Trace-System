using GT.Trace.App.Services;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Services
{
    internal class SqlPointOfUseService : IPointOfUseService
    {
        private readonly PointOfUseDao _pointsOfUse;

        public SqlPointOfUseService(PointOfUseDao pointsOfUse)
        {
            _pointsOfUse = pointsOfUse;
        }

        public async Task<bool> LoadMaterialAsync(string partNo, string workOrderCode, string etiNo, string pointOfUseCode, string componentNo)
        {
            return await _pointsOfUse.LoadEtiAsync(partNo, workOrderCode, etiNo, pointOfUseCode, componentNo).ConfigureAwait(false) > 0;
        }

        public Task<bool> LoadMaterialAsync(string lineCode, string etiInput, string pointOfUseCode)
        {
            throw new NotImplementedException();
        }
    }
}
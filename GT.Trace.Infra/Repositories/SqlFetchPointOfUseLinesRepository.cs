using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseLines;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Repositories
{
    internal class SqlFetchPointOfUseLinesRepository : IFetchPointOfUseLinesRepository
    {
        private readonly PointOfUseDao _pointsOfUse;

        public SqlFetchPointOfUseLinesRepository(PointOfUseDao pointsOfUse)
        {
            _pointsOfUse = pointsOfUse;
        }

        public async Task<string[]> FetchPointOfUseLinesAsync(string pointOfUseCode)
        {
            return await _pointsOfUse.GetPointOfUseLinesAsync(pointOfUseCode).ConfigureAwait(false);
        }
    }
}
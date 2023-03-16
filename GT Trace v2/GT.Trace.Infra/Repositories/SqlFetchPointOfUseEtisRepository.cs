using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis;
using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Repositories
{
    internal class SqlFetchPointOfUseEtisRepository : IFetchPointOfUseEtisRepository
    {
        private readonly IGttSqlDBConnection _apps;

        public SqlFetchPointOfUseEtisRepository(IGttSqlDBConnection apps)
        {
            _apps = apps;
        }

        public async Task<IEnumerable<PointOfUseEtiDto>> FetchPointOfUseEtisAsync(string pointOfUseCode)
        {
            return (await _apps.QueryAsync<PointOfUseEtis>("SELECT * FROM dbo.PointOfUseEtis WHERE UtcExpirationTime IS NULL AND UtcUsageTime IS NULL AND PointOfUseCode = @pointOfUseCode;", new { pointOfUseCode })
                .ConfigureAwait(false))
                .Select(item => new PointOfUseEtiDto(item.ID, item.EtiNo, item.ComponentNo, item.UtcEffectiveTime.ToLocalTime(), item.PartNo, item.WorkOrderCode));
        }
    }
}
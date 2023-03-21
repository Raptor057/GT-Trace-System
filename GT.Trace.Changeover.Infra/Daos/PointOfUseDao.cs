using GT.Trace.Changeover.Infra.Daos.Entities;
using GT.Trace.Common.Infra;

namespace GT.Trace.Changeover.Infra.Daos
{
    internal class PointOfUseDao : BaseDao
    {
        public PointOfUseDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<IEnumerable<PointOfUseEtis>> GetLoadedEtis(string componentNo, string pointOfUseCode) =>
            await Connection.QueryAsync<PointOfUseEtis>(
                "SELECT * FROM dbo.PointOfUseEtis WITH(NOLOCK) WHERE UtcExpirationTime IS NULL AND ComponentNo = @componentNo AND PointOfUseCode = @pointOfUseCode;",
                new { componentNo, pointOfUseCode }).ConfigureAwait(false);
    }
}
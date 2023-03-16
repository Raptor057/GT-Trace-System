using GT.Trace.Common.Infra;

namespace GT.Trace.Changeover.Infra.Daos
{
    internal class GammaDao : BaseDao
    {
        public GammaDao(ITrazaSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<IEnumerable<Entities.bom>> GetGammaAsync(string partNo, string revision) =>
            await Connection.QueryAsync<Entities.bom>(
                "SELECT CompNo, CompDesc, CompRev, CompRev2, PointOfUse FROM cegid.ufn_bom(@partNo, @revision);",
                new { revision, partNo }
                ).ConfigureAwait(false);

        public async Task<IEnumerable<Entities.bom>> GetComponentDifferences(string ogPartNo, string ogRevision, string icPartNo, string icRevision) =>
            await Connection.QueryAsync<Entities.bom>(@"SELECT CompNo, CompRev2, PointOfUse FROM cegid.ufn_bom(@ogPartNo, @ogRevision)
EXCEPT
SELECT CompNo, CompRev2, PointOfUse FROM cegid.ufn_bom(@icPartNo, @icRevision)", new { ogPartNo, icPartNo, ogRevision, icRevision }).ConfigureAwait(false);
    }
}
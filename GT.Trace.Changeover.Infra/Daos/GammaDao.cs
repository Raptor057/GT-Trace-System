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

        //Se agrego para evitar el cambio de linea si falta la gamma en la base de datos
        //RA: 07/05/2023.
        public async Task<int> GammaDataAsync(string partNo, string revision)=> await Connection.ExecuteScalarAsync<int>("SELECT CASE WHEN (SELECT COUNT([NOKTCODPF]) AS [COUNT Boom] FROM [TRAZAB].[cegid].[bom] WHERE NOKTCODPF = @partNo AND NOKTCOMPF = @revision) > 1 THEN 1 ELSE 0 END AS Result"
            , new { partNo, revision }).ConfigureAwait(false);
        //Se agrego para evitar el cambio de linea si falta la gamma en la base de datos
        //RA: 07/05/2023.
        public async Task UpdateGamaTrazabAsync(string partNo, string lineCode) =>
            await Connection.ExecuteAsync("EXECUTE [MXSRVTRACA].[TRAZAB].[dbo].[usp_update_bom_info] @partNo,@lineCode", new { partNo, lineCode }).ConfigureAwait(false);

        public async Task<IEnumerable<Entities.bom>> GetComponentDifferences(string ogPartNo, string ogRevision, string icPartNo, string icRevision) =>
            await Connection.QueryAsync<Entities.bom>(@"SELECT CompNo, CompRev2, PointOfUse FROM cegid.ufn_bom(@ogPartNo, @ogRevision)
EXCEPT
SELECT CompNo, CompRev2, PointOfUse FROM cegid.ufn_bom(@icPartNo, @icRevision)", new { ogPartNo, icPartNo, ogRevision, icRevision }).ConfigureAwait(false);
    }


}
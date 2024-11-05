using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Daos
{
    internal class BomDao : BaseDao
    {
        public BomDao(ITrazaSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<IEnumerable<BomComponent>> FetchBomAsync(string partNo, string lineCode) =>
            await Connection.QueryAsync<BomComponent>("SELECT * FROM cegid.ufn_bom(@partNo, @lineCode) ORDER BY PointOfUse, CompNo;", new { partNo, lineCode })
            .ConfigureAwait(false);

        public async Task<cegidbom?> GetLineBomEntryByComponent(string componentNo, string lineCode) =>
            await Connection.QuerySingleAsync<cegidbom?>(
                "select TOP 1 * from cegid.bom where NOCTCODECP=@componentNo and NOCTCODATE = @lineCode;",
                new { componentNo, lineCode }).ConfigureAwait(false);
        
        /*Se agrego para actualizar la gama mediante endpoint
         08/17/2023*/
        public async Task<int> UpdateGamaAsync(string partNo, string lineCode) =>
            await Connection.ExecuteAsync(
                "EXECUTE [MXSRVTRACA].[TRAZAB].[dbo].[usp_update_bom_info] @partNo,@lineCode;",
                new { partNo, lineCode }
            ).ConfigureAwait(false);
        
        public async Task DeleteGama(string partNo, string lineCode)=>
            await Connection.ExecuteAsync("Delete [TRAZAB].[cegid].[bom] WHERE NOKTCODPF = @partNo AND NOKTCOMPF = @lineCode", 
                new {partNo,lineCode}).ConfigureAwait(false);
    }
}
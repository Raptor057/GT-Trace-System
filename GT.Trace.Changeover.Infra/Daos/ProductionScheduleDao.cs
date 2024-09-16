using GT.Trace.Common.Infra;

namespace GT.Trace.Changeover.Infra.Daos
{
    internal class ProductionScheduleDao : BaseDao
    {
        public ProductionScheduleDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<int> ExpireProductionSchedule(string lineCode) =>
            await Connection.ExecuteAsync(@"UPDATE lps SET lps.UtcExpirationTime = GETUTCDATE()
        FROM dbo.LineProductionSchedule lps
        CROSS JOIN GlobalSettings gs
        WHERE lps.LineCode = @lineCode AND lps.UtcExpirationTime = gs.DefaultExpirationTime;",
                new { lineCode });


        //Agregado para corregir el BUG que no se actualiza la tabla LineProductionSchedule al aplicar cambio de modelo en cualquier linea
        public async Task<int> FindLineModelCapabilities(string lineCode, string partNo) =>
            await Connection.QueryFirstAsync<int>("SELECT COUNT([PartNo]) AS [COUNT] FROM [gtt].[dbo].[LineModelCapabilities] WHERE LineCode = @lineCode AND PartNo = @partNo;", 
                new { lineCode, partNo }).ConfigureAwait(false);

        //Agregado para completar la info de la tabla LineModelCapabilities
        //UPDATE:(MM/DD/YYYY) 9/12/2024
        //Se modifico el query para que funcionara cuando es GT o GTXXXU ya que se tenia que poner siempre y es algo tedioso.
        public async Task InsertModelCapabilities(string lineCode, string partNo) =>
            await Connection.ExecuteAsync("INSERT INTO LineModelCapabilities (PartNo,HourlyRate,LineCode)" +
                //"SELECT TOP 1 [PartNo],[HourlyRate],@lineCode FROM [gtt].[dbo].[LineModelCapabilities] WHERE PartNo = @partNo ORDER BY LineCode ASC", new { lineCode, partNo });
                "SELECT TOP 1 @partNo,[HourlyRate],@lineCode FROM [gtt].[dbo].[LineModelCapabilities] WHERE PartNo = @partNo OR PartNo = REPLACE(@partNo,'GT','') OR PartNo = REPLACE(REPLACE(@partNo,'GT',''),'U','') ORDER BY LineCode ASC", new { lineCode, partNo });

        //Agregado para completar la info de la tabla LineModelCapabilities
        //UPDATE:(MM/DD/YYYY) 9/12/2024
        //Se modifico el query para que funcionara cuando es GT o GTXXXU ya que se tenia que poner siempre y es algo tedioso.
        public async Task InsertModelCapabilitiesNew(string lineCode, string partNo) =>
            await Connection.ExecuteAsync("INSERT INTO LineModelCapabilities (PartNo,HourlyRate,LineCode) Values (@partNo, 80, @lineCode)", new { lineCode, partNo });


        //TODO: Aqui hay que ver como solucionar esto, ya que no siempre se guardan en la tabla las ordenes, y eso hace que se pierda la trazabilidad
        //BUG: En este metodo hay un Bug explicado en el TODO
        public async Task<int> ActivateProductionSchedule(string lineCode, string partNo, string workOrderCode, string revision) =>
            await Connection.ExecuteAsync(@"
        INSERT INTO dbo.LineProductionSchedule(LineCode, WorkOrderCode, PartNo, Revision, UtcEffectiveTime, UtcExpirationTime, HourlyRate)
        SELECT @lineCode, @workOrderCode, @partNo, @revision, GETUTCDATE(), gs.DefaultExpirationTime, lmc.HourlyRate FROM dbo.GlobalSettings gs
        JOIN dbo.LineModelCapabilities lmc
            ON lmc.LineCode = @lineCode AND lmc.PartNo = @partNo;",
                new { lineCode, partNo, workOrderCode, revision });

        public async Task UpdateLineGamma(string lineCode, string partNo, string revision) =>
            await Connection.ExecuteAsync(
                "EXEC [dbo].[UspUpdateLineGamma] @lineCode, @partNo, @revision;",
                new { lineCode, partNo, revision }).ConfigureAwait(false);
    }
}
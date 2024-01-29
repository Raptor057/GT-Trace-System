using GT.Trace.BomSnapShot.Infra.DataSources.Entities;

namespace GT.Trace.BomSnapShot.Infra.DataSources
{
    public class GttSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public GttSqlDB(ConfigurationSqlDbConnection<GttSqlDB> con)
        {
            _con = con;
        }

        #region TEST SNAPSHOT
        public async Task<int> CheckStatusActivecomponentsAsync(string pointOfUseCode) =>
    await _con.QueryFirstAsync<int>("SELECT [dbo].[UfnCheckStatusActiveComponents] (@pointOfUseCode) AS [StatusActiveComponents]", new { pointOfUseCode }).ConfigureAwait(false);

        public async Task<pro_production> LineProductionActiveByPointOfUseCodeAsync(string pointOfUseCode) =>
            await _con.QuerySingleAsync<pro_production>("SELECT TOP (1) PP.* FROM [gtt].[dbo].LineProcessPointsOfUse LPPOU " +
                "INNER JOIN [MXSRVTRACA].[APPS].[dbo].[pro_prod_units] PPU ON PPU.letter collate SQL_Latin1_General_CP1_CI_AS = LPPOU.LineCode " +
                "INNER JOIN [MXSRVTRACA].[APPS].[dbo].[pro_production] PP ON  PPU.id = PP.id_line AND PP.is_stoped = 0 AND PP.is_running = 1 AND PP.is_finished = 0 " +
                "WHERE LPPOU.PointOfUseCode = @pointOfUseCode", new { pointOfUseCode }).ConfigureAwait(false);
        public async Task SaveSnapShotAsync(string pointOfUseCode) =>
            await _con.ExecuteAsync("EXECUTE [dbo].[InsertComponentsSnapshot] @pointOfUseCode", new { pointOfUseCode }).ConfigureAwait(false);

        public async Task<IEnumerable<string>> LinesCodesSharingPointOfUseAsync(string pointOfUseCode, string componentNo) =>
            await _con.QueryAsync<string>("SELECT DISTINCT(s.LineCode) " +
                "FROM dbo.LineProductionSchedule s " +
                "JOIN dbo.LineGamma g ON g.PartNo = s.PartNo AND g.LineCode = s.LineCode AND g.PointOfUseCode = @pointOfUseCode AND g.CompNo = @componentNo " +
                "WHERE s.UtcExpirationTime >= GETUTCDATE() AND s.UtcEffectiveTime < GETUTCDATE()", new { pointOfUseCode, componentNo }).ConfigureAwait(false);

        public async Task<string> GetPointofusecodebyLineCode(string lineCode)=>
            await _con.QuerySingleAsync<string>("select top 1 PointOfUseCode from LinePointsOfUse WHERE LineCode = @lineCode AND PointOfUseCode NOT LIKE ('%BM%')", new { lineCode }).ConfigureAwait(false);


        #endregion
    }
}
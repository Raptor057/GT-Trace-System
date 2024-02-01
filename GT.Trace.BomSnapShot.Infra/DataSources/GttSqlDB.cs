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

        //cuenta la cantidad de componentes activos en la linea relacionando su gama para asi no tomar componentres activos de mas que pudieran estar en linea
        public async Task<int> ActiveBomComponentCountByLineCodeAsync(string lineCode) =>
        await _con.QueryFirstAsync<int>("SELECT COUNT(PUE.ComponentNo) AS [CountComponentActive]" +
            "FROM [gtt].[dbo].[PointOfUseEtis] PUE " +
            "INNER JOIN LinePointsOfUse LPU ON PUE.PointOfUseCode = LPU.PointOfUseCode " +
            "INNER JOIN LineProductionSchedule LPS ON LPS.LineCode = LPU.LineCode AND LPS.UtcExpirationTime > GETUTCDATE() " +
            "RIGHT JOIN [MXSRVTRACA].[TRAZAB].[cegid].[bom] CB ON RTRIM(CB.NOKTCODPF) COLLATE SQL_Latin1_General_CP1_CI_AS = LPS.PartNo " +
            "AND RTRIM(NOKTCOMPF) COLLATE SQL_Latin1_General_CP1_CI_AS = LPS.LineCode " +
            "AND RTRIM(CB.NOCTCODECP) COLLATE SQL_Latin1_General_CP1_CI_AS = PUE.ComponentNo " +
            "AND TRIM(CB.NOCTCODOPE) COLLATE SQL_Latin1_General_CP1_CI_AS = LPU.PointOfUseCode " +
            "WHERE LPU.LineCode = @lineCode AND PUE.UtcExpirationTime IS NULL AND PUE.UtcUsageTime IS NOT NULL AND PUE.UtcUsageTime < GETUTCDATE()", new { lineCode }).ConfigureAwait(false);

        //Ejecuta el metodo de guardado de snapshot
        public async Task SaveSnapShotAsync(string pointOfUseCode) =>
        await _con.ExecuteAsync("EXECUTE [dbo].[InsertComponentsSnapshot] @pointOfUseCode", new { pointOfUseCode }).ConfigureAwait(false);
        
        //Obtiene un punto de uso ingresando el codigo de linea este metodo se usa en caso de que un tunel y componente sea compartido por varias lineas, ejemplo grasa o silicon
        public async Task<string> GetPointofusecodebyLineCodeAsync(string lineCode)=>
        await _con.QuerySingleAsync<string>("select top 1 PointOfUseCode from LinePointsOfUse WHERE LineCode = @lineCode AND PointOfUseCode NOT LIKE ('%BM%')", new { lineCode }).ConfigureAwait(false);
        
        //Obtiene el ultimo movimiento de la etiqueta
        public async Task<PointOfUseEtis?> GetEtiLastMovementAsync(string etiNo)=>
            //await _con.QueryFirstAsync<PointOfUseEtis>("SELECT TOP 1 * FROM dbo.PointOfUseEtis WHERE EtiNo = @etiNo ORDER BY ID DESC;", new { etiNo }).ConfigureAwait(false);
            await _con.QueryFirstAsync<PointOfUseEtis>("SELECT TOP 1 * FROM dbo.PointOfUseEtis WHERE EtiNo = @etiNo AND UtcUsageTime IS NULL AND UtcExpirationTime IS NULL AND IsDepleted = 0 ORDER BY ID DESC;", new { etiNo }).ConfigureAwait(false);
    }
}
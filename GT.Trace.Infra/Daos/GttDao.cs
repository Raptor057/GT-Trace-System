using GT.Trace.Common.Infra;

namespace GT.Trace.Infra.Daos
{
    internal class GttDao : BaseDao
    {
        public GttDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<IEnumerable<dynamic>> GetLoadedEtisByLineAsync(string lineCode) =>
            await Connection.QueryAsync<dynamic>(@"SELECT pou.PointOfUseCode, e.EtiNo, e.ComponentNo, e.UtcEffectiveTime FROM dbo.LinePointsOfUse pou
JOIN dbo.PointOfUseEtis e
    ON e.PointOfUseCode = pou.PointOfUseCode AND e.UtcExpirationTime IS NULL AND e.UtcUsageTime IS NULL
WHERE pou.IsDisabled = 0 AND pou.LineCode = @lineCode;", new { lineCode });

        public async Task<IEnumerable<dynamic>> GetBomItems(string partNo, string revision) =>
            await Connection.QueryAsync<dynamic>(
                "SELECT NOCTCODOPE [PointOfUseCode], NOCTCODECP [ComponentNo], CAST(NOCTTYPOPE AS INT) [Capacity] FROM MXSRVTRACA.TRAZAB.cegid.bom WHERE NOKTCODPF = @partNo AND NOKTCOMPF = @revision;",
                new { partNo, revision });

        public async Task<dynamic> LoadBomComponentStateOrNew(string pointOfUseCode, string componentNo) =>
            await Connection.QuerySingleAsync<dynamic>(
                @"SELECT PointOfUseCode, ComponentNo, COUNT(*) [LoadSize]
FROM dbo.PointOfUseEtis
WHERE UtcExpirationTime IS NULL AND UtcUsageTime IS NULL AND PointOfUseCode = @pointOfUseCode AND ComponentNo = @componentNo
GROUP BY PointOfUseCode, ComponentNo;",
                new { pointOfUseCode, componentNo }).ConfigureAwait(false) ?? new { PointOfUseCode = pointOfUseCode, ComponentNo = componentNo, LoadSize = 0 };

        public async Task<dynamic> GetLastPointOfUseEtiEntry(string etiNo) =>
            await Connection.QueryFirst<dynamic>("SELECT TOP 1 * FROM dbo.PointOfUseEtis WHERE EtiNo = @etiNo ORDER BY UtcEffectiveTime DESC;", new { etiNo })
            .ConfigureAwait(false);

        public async Task<int> LoadEtiAsync(string etiNo, string pointOfUseCode, string componentNo, DateTime effectiveTime) =>
            await Connection.ExecuteAsync(
                "INSERT INTO dbo.PointOfUseEtis (EtiNo, PointOfUseCode, ComponentNo, UtcEffectiveTime) VALUES(@etiNo, @pointOfUseCode, @componentNo, @utcEffectiveTime);",
                new { etiNo, pointOfUseCode, componentNo, utcEffectiveTime = effectiveTime.ToUniversalTime() }
            ).ConfigureAwait(false);

        public async Task<int> UnloadEtiAsync(string etiNo, DateTime expirationTime) =>
            await Connection.ExecuteAsync(
                "UPDATE dbo.PointOfUseEtis SET UtcExpirationTime = @utcExpirationTime WHERE EtiNo = @etiNo AND UtcEffectiveTime <= GETUTCDATE() AND UtcUsageTime IS NULL AND UtcExpirationTime IS NULL;",
                new { etiNo, utcExpirationTime = expirationTime.ToUniversalTime() }
            ).ConfigureAwait(false);

        public async Task<int> ReturnEtiAsync(string etiNo, bool isDepleted, DateTime expirationTime) =>
            await Connection.ExecuteAsync(
                "UPDATE dbo.PointOfUseEtis SET UtcExpirationTime = @utcExpirationTime, IsDepleted = @isDepleted WHERE EtiNo = @etiNo AND UtcUsageTime <= GETUTCDATE() AND UtcExpirationTime IS NULL;",
                new { etiNo, isDepleted, utcExpirationTime = expirationTime.ToUniversalTime() }
            ).ConfigureAwait(false);

        public async Task<int> UseEtiAsync(string etiNo, DateTime usageTime) =>
            await Connection.ExecuteAsync(
                "UPDATE dbo.PointOfUseEtis SET UtcUsageTime = @utcUsageTime WHERE EtiNo = @etiNo AND UtcEffectiveTime <= GETUTCDATE() AND UtcUsageTime IS NULL AND UtcExpirationTime IS NULL;",
                new { etiNo, utcUsageTime = usageTime.ToUniversalTime() }
            ).ConfigureAwait(false);

        public async Task<int> UpdateGamaAsync(string partNo, string lineCode) =>
            await Connection.ExecuteAsync(
                "EXEC dbo.UspUpdateLineGamma @lineCode, @partNo, @lineCode;",
                new { partNo, lineCode }
            ).ConfigureAwait(false);


        /*Se agrego esto para crear un endpoint que actualize el la gama de los componentes Agregado el 8/17/2023 */
        #region 
        //public async Task<dynamic> GetLineandPartnofromPointOfUseAsync(string PointOfUse) =>
        //    await Connection.QueryFirst<dynamic>
        //    ("SELECT top 1 LP.LineCode,LPS.PartNo FROM PointOfUseEtis PO LEFT JOIN LinePointsOfUse LP ON LP.PointOfUseCode = PO.PointOfUseCode LEFT JOIN LineProductionSchedule LPS ON LPS.LineCode = LP.LineCode WHERE PO.PointOfUseCode = @PointOfUse AND PO.UtcExpirationTime IS NULL AND PO.UtcUsageTime IS NOT NULL AND LPS.UtcExpirationTime > GETUTCDATE();"
        //        , new { PointOfUse }).ConfigureAwait(false);
        #endregion
    }
}
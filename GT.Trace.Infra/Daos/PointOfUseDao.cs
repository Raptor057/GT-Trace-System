using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Daos
{
    internal class PointOfUseDao : BaseDao
    {
        public PointOfUseDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<IEnumerable<PointOfUseEtis>> FetchAllLoadedEtisAsync(string lineCode) =>
            await Connection.QueryAsync<PointOfUseEtis>(
                @"SELECT poue.*
FROM dbo.LinePointsOfUse lpou
JOIN dbo.PointOfUseEtis poue
    ON poue.PointOfUseCode = lpou.PointOfUseCode
    AND poue.UtcEffectiveTime <= GETUTCDATE() AND poue.UtcUsageTime IS NULL AND poue.UtcExpirationTime IS NULL
WHERE lpou.LineCode = @lineCode;",
                new { lineCode }
            ).ConfigureAwait(false);

        public async Task<IEnumerable<PointOfUseEtis>> FetchLoadedEtisByWorkOrderAsync(string lineCode, string partNo, string workOrderCode) =>
            await Connection.QueryAsync<PointOfUseEtis>(
                @"SELECT poue.*
FROM dbo.LinePointsOfUse lpou
JOIN dbo.PointOfUseEtis poue
    ON poue.PointOfUseCode = lpou.PointOfUseCode AND poue.PartNo = @partNo AND poue.WorkOrderCode = @workOrderCode
    AND poue.UtcEffectiveTime <= GETUTCDATE() AND poue.UtcUsageTime IS NULL AND poue.UtcExpirationTime IS NULL
WHERE lpou.LineCode = @lineCode;",
                new { lineCode, partNo, workOrderCode }
            ).ConfigureAwait(false);

        public async Task<IEnumerable<PointOfUseEtis>> FindActiveEtisAsync(string lineCode, string partNo, string workOrderCode) =>
            await Connection.QueryAsync<PointOfUseEtis>(
                @"SELECT poue.*, c.counter_value [PackingCount], c.bin_size [Size]
FROM dbo.LinePointsOfUse lpou
JOIN dbo.PointOfUseEtis poue
    ON poue.PointOfUseCode = lpou.PointOfUseCode
    AND poue.UtcUsageTime <= GETUTCDATE() AND poue.UtcExpirationTime IS NULL
LEFT JOIN [MXSRVTRACA].[TRAZAB].[dbo].[eti_packing_counters] c
    ON c.eti_no COLLATE SQL_Latin1_General_CP1_CI_AS = poue.EtiNo
WHERE lpou.LineCode = @lineCode;",
                new { lineCode, partNo, workOrderCode }
            ).ConfigureAwait(false);

        public async Task<int> LoadEtiAsync(string partNo, string workOrderCode, string etiNo, string pointOfUseCode, string componentNo) =>
            await Connection.ExecuteAsync(
                "INSERT INTO dbo.PointOfUseEtis (PartNo, WorkOrderCode, EtiNo, PointOfUseCode, ComponentNo) VALUES(@partNo, @workOrderCode, @etiNo, @pointOfUseCode, @componentNo);",
                new { partNo, workOrderCode, etiNo, pointOfUseCode, componentNo }
            ).ConfigureAwait(false);

        public async Task<int> UnloadEtiAsync(string partNo, string workOrderCode, string etiNo) =>
            await Connection.ExecuteAsync(
                "UPDATE dbo.PointOfUseEtis SET UtcExpirationTime = GETUTCDATE() WHERE EtiNo = @etiNo AND UtcEffectiveTime <= GETUTCDATE() AND UtcUsageTime IS NULL AND UtcExpirationTime IS NULL;",
                new { partNo, workOrderCode, etiNo }
            ).ConfigureAwait(false);

        public async Task<int> UnloadEtiAsync(string etiNo, string pointOfUseCode) =>
            await Connection.ExecuteAsync(
                "UPDATE dbo.PointOfUseEtis SET UtcExpirationTime = GETUTCDATE() WHERE EtiNo = @etiNo AND PointOfUseCode = @pointOfUseCode AND UtcEffectiveTime <= GETUTCDATE() AND UtcUsageTime IS NULL AND UtcExpirationTime IS NULL;",
                new { etiNo, pointOfUseCode }
            ).ConfigureAwait(false);

        public async Task<int> ReturnEtiAsync(string partNo, string workOrderCode, string etiNo, bool isDepleted) =>
            await Connection.ExecuteAsync(
                "UPDATE dbo.PointOfUseEtis SET UtcExpirationTime = GETUTCDATE(), IsDepleted = @isDepleted WHERE PartNo = @partNo AND WorkOrderCode = @workOrderCode AND EtiNo = @etiNo AND UtcUsageTime <= GETUTCDATE() AND UtcExpirationTime IS NULL;",
                new { partNo, workOrderCode, etiNo, isDepleted }
            ).ConfigureAwait(false);

        public async Task<int> UseEtiAsync(string partNo, string workOrderCode, string etiNo, string pointOfUseCode) =>
            await Connection.ExecuteAsync(
                "UPDATE dbo.PointOfUseEtis SET UtcUsageTime = GETUTCDATE() WHERE PointOfUseCode = @pointOfUseCode AND PartNo = @partNo AND WorkOrderCode = @workOrderCode AND EtiNo = @etiNo AND UtcEffectiveTime <= GETUTCDATE() AND UtcUsageTime IS NULL AND UtcExpirationTime IS NULL;",
                new { partNo, workOrderCode, etiNo, pointOfUseCode }
            ).ConfigureAwait(false);

        public async Task<PointOfUseEtis> GetEtiInfoAsync(string etiNo) =>
            await Connection.QuerySingleAsync<PointOfUseEtis>(
                "SELECT TOP 1 * FROM dbo.PointOfUseEtis WHERE EtiNo = @etiNo AND (UtcExpirationTime IS NULL OR (UtcExpirationTime IS NOT NULL AND IsDepleted = 1)) ORDER BY ID DESC;",
                new { etiNo }
            ).ConfigureAwait(false);

        public async Task<string[]> GetPointOfUseLinesAsync(string pointOfUseCode) =>
            (await Connection.QueryAsync<dynamic>("SELECT * FROM dbo.LinePointsOfUse WHERE IsDisabled = 0 AND PointOfUseCode = @pointOfUseCode;", new { pointOfUseCode }).ConfigureAwait(false))
            .Select(item => (string)item.LineCode).ToArray();
    }
}
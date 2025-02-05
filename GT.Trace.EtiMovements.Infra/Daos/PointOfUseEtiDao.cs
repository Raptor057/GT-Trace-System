using GT.Trace.Common.Infra;
using GT.Trace.EtiMovements.Infra.Entities;

namespace GT.Trace.EtiMovements.Infra.Daos
{
    internal sealed class PointOfUseEtiDao : BaseDao
    {
        public PointOfUseEtiDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        /// <summary>
        /// Original 
        /// Not Used anymore pleace use V2
        /// </summary>
        /// <param name="pointOfUseCode"></param>
        /// <param name="componentNo"></param>
        /// <returns>int number</returns>
        [Obsolete]
        public async Task<int> CountLinesSharingPointOfUseAsync(string pointOfUseCode, string componentNo) =>
            await Connection.ExecuteScalarAsync<int>(@"SELECT COUNT(*) FROM dbo.LineProductionSchedule s
        JOIN dbo.LineGamma g
            ON g.PartNo = s.PartNo AND g.LineCode = s.LineCode AND g.PointOfUseCode = @pointOfUseCode AND g.CompNo = @componentNo
        WHERE s.UtcExpirationTime >= '2099-12-31 23:59:59.997';", new { pointOfUseCode, componentNo }).ConfigureAwait(false);

        /// <summary>
        /// Not Used anymore pleace use V3
        /// </summary>
        /// <param name="pointOfUseCode"></param>
        /// <param name="componentNo"></param>
        /// <returns>int number</returns>
        [Obsolete]
        public async Task<int> CountLinesSharingPointOfUseAsyncV2(string pointOfUseCode, string componentNo) =>
    await Connection.ExecuteScalarAsync<int>(@"
            SELECT COUNT(*) FROM dbo.LineProductionSchedule s
            JOIN dbo.LineGamma g
            ON g.PartNo = s.PartNo AND g.LineCode = s.LineCode AND g.PointOfUseCode = @pointOfUseCode AND g.CompNo = @componentNo
            WHERE s.UtcExpirationTime > GETUTCDATE();"
            , new { pointOfUseCode, componentNo }).ConfigureAwait(false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointOfUseCode"></param>
        /// <param name="componentNo"></param>
        /// <returns>int number</returns>
        public async Task<int> CountLinesSharingPointOfUseAsyncV3(string pointOfUseCode, string componentNo) =>
    await Connection.ExecuteScalarAsync<int>(@"
            SELECT COUNT(DISTINCT s.LineCode) FROM dbo.LineProductionSchedule s
            JOIN dbo.LineGamma g
            ON g.PartNo = s.PartNo AND g.LineCode = s.LineCode AND g.PointOfUseCode = @pointOfUseCode AND g.CompNo = @componentNo
            WHERE s.UtcExpirationTime > GETUTCDATE();"
            , new { pointOfUseCode, componentNo }).ConfigureAwait(false);

        public async Task<int> AddAsync(PointOfUseEtis movement)
        {
            return await Connection.ExecuteAsync(
                "INSERT INTO dbo.PointOfUseEtis (PointOfUseCode, EtiNo, ComponentNo, UtcEffectiveTime, LotNo) VALUES(@PointOfUseCode, @EtiNo, @ComponentNo, @UtcEffectiveTime, @LotNo);" +
                "EXECUTE [dbo].[UpsCopyEtiInV2] @EtiNo;",//Esta linea agrega en la tabla [PointOfUseEtisV2] el dato recien agregado, se hizo asi ya que en el update se utiliza
                                                         //el ID para actualizar y no el numero de eti.
                movement
            ).ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(PointOfUseEtis movement)
        {
            return await Connection.ExecuteAsync(
                "UPDATE dbo.PointOfUseEtis SET UtcEffectiveTime = @UtcEffectiveTime, UtcUsageTime = @UtcUsageTime, UtcExpirationTime = @UtcExpirationTime, IsDepleted = @IsDepleted WHERE ID = @ID;" +
                "UPDATE dbo.PointOfUseEtisV2 SET UtcEffectiveTime = @UtcEffectiveTime, UtcUsageTime = @UtcUsageTime, UtcExpirationTime = @UtcExpirationTime, IsDepleted = @IsDepleted WHERE ID = @ID;",
                movement
            ).ConfigureAwait(false);
        }

        public async Task<PointOfUseEtis?> GetEtiLastMovementAsync(string etiNo)
        {
            return await Connection.QueryFirstAsync<PointOfUseEtis>("SELECT TOP 1 * FROM dbo.PointOfUseEtis WHERE EtiNo = @etiNo ORDER BY ID DESC;", new { etiNo }).ConfigureAwait(false);
        }


        //agregado para guardar las etis removidas en la tabla SaveRemoveEtis
        [Obsolete]
        public async Task<int> SaveRemoveEtiAsync(PointOfUseEtis movement)
        {
            return await Connection.ExecuteAsync("EXEC InsertSaveRemoveEti @etiNo;", movement).ConfigureAwait(false);
        }

        [Obsolete]
        public async Task<int> WagonLoadaAsync (string PointOfUseCode, string ComponentNo)
        {
            //En esta tabla se guardan los componentes consumidos con estatus IsDepleted = 1
            return await Connection.ExecuteScalarAsync<int>("INSERT INTO [dbo].[WagonLoad] ([PointOfUseCode],[ComponentNo]) VALUES (@PointOfUseCode,@ComponentNo)",
                new { PointOfUseCode, ComponentNo}).ConfigureAwait(false);
        }


        public async Task<PointOfUseEtis?> GetLastUsedNotReturnedEtiAsync(string pointOfUseCode, string componentNo)
        {
            return await Connection.QueryFirstAsync<PointOfUseEtis>(
                "SELECT TOP 1 * FROM dbo.PointOfUseEtis WHERE PointOfUseCode = @pointOfUseCode AND ComponentNo = @componentNo AND UtcUsageTime IS NOT NULL AND UtcExpirationTime IS NULL ORDER BY ID DESC;",
                new { pointOfUseCode, componentNo }
            ).ConfigureAwait(false);
        }

        public async Task<int> GetTotalComponentEtisInPoingOfUse(string pointOfUseCode, string componentNo)
        {
            return await Connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM dbo.PointOfUseEtis WHERE PointOfUseCode = @pointOfUseCode AND ComponentNo = @componentNo AND UtcExpirationTime IS NULL AND UtcUsageTime IS NULL;",
                new { pointOfUseCode, componentNo }
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<string>> GetPointOfUseLoadedEtisAsync(string pointOfUseCode, string componentNo)
        {
            var loadedEtis = await Connection.QueryAsync<dynamic>(
                "SELECT EtiNo FROM dbo.PointOfUseEtis WHERE PointOfUseCode = @pointOfUseCode AND ComponentNo = @componentNo AND UtcExpirationTime IS NULL AND UtcUsageTime IS NULL;",
                new { pointOfUseCode, componentNo }
            ).ConfigureAwait(false);
            return loadedEtis.Select(item => (string)item.EtiNo);
        }

        public async Task<IEnumerable<PointOfUseEtis>> GetLoadedEtis(IEnumerable<string> pointsOfUseCode)
        {
            return await Connection.QueryAsync<PointOfUseEtis>(
                "SELECT * FROM dbo.PointOfUseEtis WHERE PointOfUseCode IN @pointsOfUseCode AND UtcExpirationTime IS NULL AND UtcUsageTime IS NULL;",
                new { pointsOfUseCode }
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<PointOfUseEtis>> GetActiveEtis(IEnumerable<string> pointsOfUseCode)
        {
            return await Connection.QueryAsync<PointOfUseEtis>(
                "SELECT * FROM dbo.PointOfUseEtis WHERE PointOfUseCode IN @pointsOfUseCode AND UtcExpirationTime IS NULL AND UtcUsageTime IS NOT NULL;",
                new { pointsOfUseCode }
            ).ConfigureAwait(false);
        }
    }
}
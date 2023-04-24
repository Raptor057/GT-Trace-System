using GT.Trace.Packaging.Infra.DataSources.Entities;

namespace GT.Trace.Packaging.Infra.DataSources
{
    public class GttSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public GttSqlDB(ConfigurationSqlDbConnection<GttSqlDB> con)
        {
            _con = con;
        }

        public async Task<LineShiftHeadcount> GetLineHeadcountAsync(string lineCode) =>
            await _con.QuerySingleAsync<LineShiftHeadcount>("SELECT * FROM dbo.UfnLineShiftHeadcount(@lineCode);", new { lineCode }).ConfigureAwait(false);

        public async Task<IEnumerable<ProductionTimeInterval>> GetTimeIntervalProduction(string lineCode, int hourlyRate) =>
            await _con.QueryAsync<ProductionTimeInterval>(
                "SELECT * FROM dbo.UfnProductionTimeIntervals(@lineCode, NULL, @hourlyRate);",
                new { lineCode, hourlyRate }).ConfigureAwait(false);

        /// <summary>
        /// Add the packaging process to the unit's process history.
        /// </summary>
        /// <remarks>The value 999 is the packaging process ID.</remarks>
        /// <param name="unitID">The unit's ID.</param>
        /// <param name="lineCode">Two-character line code.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RecordPackagingProcessHistoryAsync(long unitID, string lineCode) =>
            await _con.ExecuteAsync("INSERT INTO dbo.ProcessHistory (UnitID, ProcessID, LineCode) VALUES(@unitID, '999', @lineCode);", new { unitID, lineCode })
            .ConfigureAwait(false);

        /// <summary>
        /// Record production.
        /// </summary>
        /// <param name="lineCode">The line code.</param>
        /// <param name="partNo">The model's part number.</param>
        /// <param name="revision">The model's revision.</param>
        /// <param name="workOrderCode">The work order code.</param>
        /// <param name="quantity">The produced quantity.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RecordProductionAsync(string lineCode, string partNo, string revision, string workOrderCode, int quantity) =>
            await _con.ExecuteAsync(
                "INSERT INTO ProductionLogs (LineCode, PartNo, Revision, WorkOrderCode, Quantity) VALUES(@lineCode, @partNo, @revision, @workOrderCode, @quantity);",
                new { lineCode, partNo, revision, workOrderCode, quantity })
            .ConfigureAwait(false);

        public async Task<string> GetActiveWorkOrderAsync(string lineCode) =>
            await _con.ExecuteScalarAsync<string>(
                "SELECT WorkOrderCode FROM dbo.LineProductionSchedule WHERE LineCode = @lineCode AND UtcEffectiveTime <= GETUTCDATE() AND UtcExpirationTime > GETUTCDATE();",
                new { lineCode }).ConfigureAwait(false);

        public async Task<IEnumerable<PointOfUseEtis>> GetActiveSetByLineAsync(string lineCode) =>
            await _con.QueryAsync<PointOfUseEtis>(@"SELECT e.* FROM dbo.LinePointsOfUse p
JOIN dbo.PointOfUseEtis e
    ON e.PointOfUseCode = p.PointOfUseCode AND e.UtcUsageTime IS NOT NULL AND e.UtcExpirationTime IS NULL
WHERE LineCode = @lineCode;", new { lineCode }).ConfigureAwait(false);

        internal async Task<IEnumerable<LineRouting>> GetLineRoutingByLineCode(string lineCode) =>
            await _con.QueryAsync<LineRouting>("SELECT * FROM LineRouting WHERE LineCode=@lineCode;", new { lineCode }).ConfigureAwait(false);

        //Esto es nuevo para la trazabilidad
        public async Task AddTracedUnitAsync(long unitID, string partNo, string lineCode, string workOrderCode) =>
            await _con.ExecuteAsync("EXEC UpsInsertProductionTraceability @unitID, @partNo, @lineCode, @workOrderCode;", new { unitID, partNo , lineCode, workOrderCode }).ConfigureAwait(false);

        //Se agrego para obtener el ultimo mensaje de la linea, se piensa usar para el bloqueo de linea, que de la info y se imprima en la pantalla
        public async Task<string> GetMessageFromAssembly(string linecode) =>
            await _con.ExecuteScalarAsync<string>("SELECT top 1 ClientMessage FROM [gtt].[dbo].[EventsHistory] where " +
                "ClientMessage like ('%corresponde con ningún punto de uso para la gama%') and LineCode = @linecode order BY UtcTimeStamp desc", new { linecode }).ConfigureAwait(false);
        //public async Task<string> GetMessageFromAssembly(string linecode) =>
        //    await _con.ExecuteScalarAsync<string>("SELECT top 1 ClientMessage FROM [gtt].[dbo].[EventsHistory] where LineCode = @linecode order BY UtcTimeStamp desc", new { linecode }).ConfigureAwait(false);
    }
}
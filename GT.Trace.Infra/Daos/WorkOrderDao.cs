using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Daos
{
    internal class WorkOrderDao : BaseDao
    {
        private readonly IGttSqlDBConnection _gtt;

        public WorkOrderDao(IAppsSqlDBConnection connection, IGttSqlDBConnection gtt)
            : base(connection)
        {
            _gtt = gtt;
        }

        public async Task UpdateWorkOrderQuantity(int lineID, string workOrderCode, int quantity) =>
            await Connection.ExecuteAsync(
                "UPDATE pro_production SET current_qty = @quantity WHERE codew=@workOrderCode AND id_line = @lineID;",
                new { workOrderCode, lineID, quantity }).ConfigureAwait(false);

        public async Task<pro_production> GetWorkOrderByCodeAsync(int lineID, string workOrderCode) =>
            await Connection.QuerySingleAsync<pro_production>(
                "SELECT * FROM pro_production WHERE codew=@workOrderCode AND id_line = @lineID AND is_running=1 AND is_stoped = 0 AND is_finished=0;",
                new { lineID, workOrderCode }).ConfigureAwait(false);

        public async Task<pro_production> GetWorkOrderByCodeAsync(string workOrderCode) =>
            await Connection.QuerySingleAsync<pro_production>(
                "SELECT TOP 1 * FROM pro_production WHERE codew=@workOrderCode ORDER BY id DESC;",
                new { workOrderCode }).ConfigureAwait(false);

        public async Task<string> GetActiveWorkOrderAsync(string lineCode) =>
            await _gtt.ExecuteScalarAsync<string>(
                "SELECT WorkOrderCode FROM dbo.LineProductionSchedule WHERE LineCode = @lineCode AND UtcEffectiveTime <= GETUTCDATE() AND UtcExpirationTime > GETUTCDATE();",
                new { lineCode }).ConfigureAwait(false);

        public async Task<pro_production> GetWorkOrderRunningByLineAsync(int lineID) =>
            await Connection.QuerySingleAsync<pro_production>(
        @"SELECT TOP 1 *
FROM dbo.pro_production p
WHERE p.id_line = @lineID and p.is_running = 1 AND p.is_finished = 0 AND p.is_stoped = 0
ORDER BY last_update_time DESC;",
        new { lineID }).ConfigureAwait(false);
    }
}
using GT.Trace.Common.Infra;

namespace GT.Trace.Changeover.Infra.Daos
{
    internal class WorkOrderDao : BaseDao
    {
        public WorkOrderDao(IAppsSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<Entities.pro_production> GetActiveWorkOrderByLine(int lineID) =>
            await Connection.QuerySingleAsync<Entities.pro_production>(
                "SELECT TOP 1 * FROM dbo.pro_production WHERE id_line=@lineID AND is_running=1 AND is_stoped=0 AND is_finished=0 ORDER BY last_update_time DESC;",
                new { lineID }
                ).ConfigureAwait(false);
    }
}
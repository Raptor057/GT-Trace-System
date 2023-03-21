using GT.Trace.App.UseCases.MaterialLoading.FetchLineWorkOrders;
using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Repositories
{
    internal class SqlFetchLineWorkOrdersRepository : IFetchLineWorkOrdersRepository
    {
        private readonly IAppsSqlDBConnection _apps;

        public SqlFetchLineWorkOrdersRepository(IAppsSqlDBConnection apps)
        {
            _apps = apps;
        }

        public async Task<IEnumerable<WorkOrderDto>> FetchWorkOrdersByLineAsync(int lineID)
        {
            return (await _apps.QueryAsync<pro_production>("SELECT * FROM dbo.pro_production WHERE id_line = @lineID and is_finished = 0;", new { lineID })
                .ConfigureAwait(false))
                .Select(item => new WorkOrderDto(item.id, item.codew, item.part_number.Trim(), item.rev.Trim(), item.order, item.line, item.client_name.Trim()));
        }
    }
}
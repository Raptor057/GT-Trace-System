using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Daos
{
    internal class ProductionDao : BaseDao
    {
        public ProductionDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        public async Task AddNewProductionLogAsync(ProductionLogs entity) =>
            await Connection.QuerySingleAsync<pro_subeti>(
                "INSERT INTO ProductionLogs (LineCode, PartNo, Revision, WorkOrderCode, Quantity) VALUES(@LineCode, @PartNo, @Revision, @WorkOrderCode, @Quantity);",
                entity)
            .ConfigureAwait(false);

        public async Task<IEnumerable<LineHourlyProductionByWorkDay>> GetLineHourlyProductionByWorkDayAsync(string lineCode, DateTime? workDayDate = null) =>
            await Connection.QueryAsync<LineHourlyProductionByWorkDay>(
                "SELECT * FROM dbo.UfnLineHourlyProductionByWorkDay (@lineCode, @workDayDate);",
                new { lineCode, workDayDate }).ConfigureAwait(false);

        public async Task<LineCurrentHourProduction> GetLineCurrentHourProductionAsync(string lineCode) =>
            await Connection.QuerySingleAsync<LineCurrentHourProduction>(
                "SELECT * FROM dbo.UfnLineCurrentHourProduction (@lineCode);",
                new { lineCode }).ConfigureAwait(false);
    }
}
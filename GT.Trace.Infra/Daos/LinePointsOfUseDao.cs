using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Daos
{
    internal class LinePointsOfUseDao : BaseDao
    {
        public LinePointsOfUseDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<IEnumerable<LinePointOfUse>> FindLinePointsOfUseAsync(string lineCode, bool canBeLoadedByOperations, bool isDisabled) =>
            await Connection.QueryAsync<LinePointOfUse>(
                "SELECT * FROM dbo.LinePointsOfUse WHERE CanBeLoadedByOperations = @canBeLoadedByOperations AND LineCode = @lineCode AND IsDisabled = @isDisabled;",
                new { lineCode, canBeLoadedByOperations, isDisabled }).ConfigureAwait(false);

        public async Task<IEnumerable<LinePointOfUse>> FindLinePointsOfUseAsync(string lineCode, bool isDisabled) =>
            await Connection.QueryAsync<LinePointOfUse>(
                "SELECT * FROM dbo.LinePointsOfUse WHERE LineCode = @lineCode AND IsDisabled = @isDisabled;",
                new { lineCode, isDisabled }).ConfigureAwait(false);
    }
}
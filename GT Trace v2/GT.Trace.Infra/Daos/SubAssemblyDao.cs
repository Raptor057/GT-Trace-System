using GT.Trace.Common.Infra;

namespace GT.Trace.Infra.Daos
{
    internal class SubAssemblyDao : BaseDao
    {
        public SubAssemblyDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<long> AddSubAssemblyAsync(string lineCode, string componentNo, string revision, string workOrderCode, int quantity) =>
            await Connection.ExecuteScalarAsync<long>(
                "INSERT INTO dbo.SubAssemblies(LineCode, ComponentNo, Revision, WorkOrderCode, Quantity) VALUES(@lineCode, @componentNo, @revision, @workOrderCode, @quantity); SELECT SCOPE_IDENTITY();",
                new { lineCode, componentNo, revision, workOrderCode, quantity })
            .ConfigureAwait(false);
    }
}
using GT.Trace.Common.Infra;

namespace GT.Trace.Changeover.Infra.Daos
{
    internal class LineDao : BaseDao
    {
        public LineDao(IAppsSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<Entities.pro_prod_units> GetLineByCode(string lineCode) =>
            await Connection.QuerySingleAsync<Entities.pro_prod_units>(
                "SELECT * FROM dbo.pro_prod_units WHERE letter=@lineCode;",
                new { lineCode }
                ).ConfigureAwait(false);

        public async Task Update(int lineID, string workOrderCode, string partNo, string revision) =>
            await Connection.ExecuteAsync("update apps.dbo.pro_prod_units set modelo=RTRIM(@partNo), active_revision=RTRIM(@revision), codew=@workOrderCode where id=@lineID;", new { workOrderCode, partNo, revision, lineID }).ConfigureAwait(false);
    }
}
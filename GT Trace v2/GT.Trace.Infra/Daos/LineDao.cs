using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Daos
{
    internal class LineDao : BaseDao
    {
        public LineDao(IAppsSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<pro_prod_units> GetLineByLineCodeAsync(string lineCode) =>
            await Connection.QuerySingleAsync<pro_prod_units>("SELECT * FROM APPS.dbo.pro_prod_units WHERE letter=@lineCode;", new { lineCode })
            .ConfigureAwait(false);
    }
}
using GT.Trace.Common.Infra;
using GT.Trace.Etis.Infra.Entities;

namespace GT.Trace.Etis.Infra.Daos
{
    internal class EtiDao : BaseDao
    {
        public EtiDao(IAppsSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<pro_eti001?> TryGetEtiByIDAsync(long etiID) =>
            await Connection.QuerySingleAsync<pro_eti001?>("SELECT * FROM pro_ETI001 WHERE iD=@etiID;", new { etiID })
                .ConfigureAwait(false);
    }
}
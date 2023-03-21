using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Daos
{
    internal class SubEtiDao : BaseDao
    {
        public SubEtiDao(ITrazaSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<pro_subeti> GetSubEtiByIDAsync(long id) =>
            await Connection.QuerySingleAsync<pro_subeti>("SELECT * FROM pro_SUBETI WHERE id=@id;", new { id })
            .ConfigureAwait(false);
    }
}
using GT.Trace.Common.Infra;
using GT.Trace.Etis.Infra.Entities;

namespace GT.Trace.Etis.Infra.Daos
{
    internal class MotorsEtiDao : BaseDao
    {
        public MotorsEtiDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<SubAssemblyEti?> TryGetEtiByIDAsync(long id) =>
            await Connection.QuerySingleAsync<SubAssemblyEti?>("SELECT * FROM dbo.SubAssemblies WHERE SubAssemblyID=@id;", new { id })
                .ConfigureAwait(false);
    }
}
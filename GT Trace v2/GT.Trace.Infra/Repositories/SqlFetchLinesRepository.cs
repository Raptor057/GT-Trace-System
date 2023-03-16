using GT.Trace.App.UseCases.MaterialLoading.FetchLines;
using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Repositories
{
    internal class SqlFetchLinesRepository : IFetchLinesRepository
    {
        private readonly IAppsSqlDBConnection _apps;

        public SqlFetchLinesRepository(IAppsSqlDBConnection apps)
        {
            _apps = apps;
        }

        public async Task<IEnumerable<LineDto>> FetchLinesAsync()
        {
            return (await _apps.QueryAsync<pro_prod_units>("SELECT * FROM dbo.pro_prod_units;")
                .ConfigureAwait(false))
                .Select(item => new LineDto(item.id, item.letter, item.comments, item.modelo, item.active_revision, item.codew));
        }
    }
}
using GT.Trace.EZ2000.Packaging.App.UseCases.UpdateActiveEti;
using GT.Trace.EZ2000.Packaging.Infra.DataSources;

namespace GT.Trace.EZ2000.Packaging.Infra.Gateways
{
    internal class SqlUpdateActiveEtiGateway : IUpdateActiveEti
    {
        private readonly ConfigurationSqlDbConnection<GttSqlDB> _gtt;

        public SqlUpdateActiveEtiGateway(ConfigurationSqlDbConnection<GttSqlDB> gtt)
        {
            _gtt=gtt;
        }

        public async Task UpdateActiveEtiAsync(string etiNo)
        {
            //await _gtt.ExecuteAsync("EXEC [dbo].[UpsUpdateActiveEtis] @EtiNo", new { etiNo}).ConfigureAwait(false);
            await _gtt.ExecuteAsync("EXEC UpsUpdateActiveEtis @EtiNo", new { etiNo }).ConfigureAwait(false);
        }
    }
}

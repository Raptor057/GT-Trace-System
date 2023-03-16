using GT.Trace.EZ2000.Packaging.App.UseCases.SetLineHeadcount;
using GT.Trace.EZ2000.Packaging.Infra.DataSources;

namespace GT.Trace.EZ2000.Packaging.Infra.Gateways
{
    internal class SqlLineHeadcountGateway : ILineHeadcountGateway
    {
        private readonly ConfigurationSqlDbConnection<GttSqlDB> _gtt;

        public SqlLineHeadcountGateway(ConfigurationSqlDbConnection<GttSqlDB> gtt)
        {
            _gtt = gtt;
        }

        public async Task SetCurrentShiftWorkOrderHeadcountAsync(string lineCode, string workOrderCode, int headcount)
        {
            await _gtt.ExecuteAsync("EXEC UspSetLineHeadcount @lineCode, @workOrderCode, @headcount", new { lineCode, workOrderCode, headcount }).ConfigureAwait(false);
        }
    }
}
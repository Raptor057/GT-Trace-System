using GT.Trace.EZ2000.Packaging.App.Gateways;

namespace GT.Trace.EZ2000.Packaging.Infra.Gateways
{
    internal sealed class SqlMasterLabelsGateway : IMasterLabelsGateway
    {
        private readonly DataSources.TrazaSqlDB _traza;

        public SqlMasterLabelsGateway(DataSources.TrazaSqlDB traza)
        {
            _traza = traza;
        }

        public async Task<long?> GetLastMasterFolioByLineAsync(string lineName)
        {
            return await _traza.GetLastMasterFolioByLineAsync(lineName).ConfigureAwait(false);
        }
    }
}
using GT.Trace.Changeover.App.Dtos;
using GT.Trace.Changeover.App.Gateways;
using GT.Trace.Changeover.Infra.Daos;

namespace GT.Trace.Changeover.Infra.Gateways
{
    internal class SqlGammaGateway : IGammaGateway
    {
        private readonly GammaDao _gamma;

        public SqlGammaGateway(GammaDao gamma)
        {
            _gamma = gamma;
        }

        public async Task<IEnumerable<GammaItemDto>> GetGammaAsync(string lineCode, string partNo, string revision)
        {
            return (await _gamma.GetGammaAsync(partNo, revision).ConfigureAwait(false))
                .Select(item => new GammaItemDto(item.PointOfUse, item.CompNo, item.CompRev, item.CompRev2, item.CompDesc));
        }

        public async Task<IEnumerable<GammaItemDto>> GetOutgoingComponentsAsync(string ogPartNo, string ogRevision, string icPartNo, string icRevision)
        {
            return (await _gamma.GetComponentDifferences(ogPartNo, ogRevision, icPartNo, icRevision).ConfigureAwait(false))
                .Select(item => new GammaItemDto(item.PointOfUse, item.CompNo, item.CompRev, item.CompRev2, item.CompDesc));
        }
    }
}
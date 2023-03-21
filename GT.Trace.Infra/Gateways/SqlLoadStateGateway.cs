using GT.Trace.App.UseCases.Lines.GetLoadState;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Gateways
{
    internal class SqlLoadStateGateway : ILoadStateGateway
    {
        private readonly GttDao _gtt;

        private readonly BomDao _bom;

        public SqlLoadStateGateway(GttDao gtt, BomDao bom)
        {
            _gtt = gtt;
            _bom = bom;
        }

        public async Task<IEnumerable<GamaEntryDto>> GetGamaAsync(string partNo, string revision) =>
            (await _bom.FetchBomAsync(partNo, revision).ConfigureAwait(false))
            .Select(item => new GamaEntryDto(item.PointOfUse, item.CompNo, int.TryParse(item.Capacity, out var capacity) ? capacity : -1));

        public async Task<IEnumerable<PointOfUseEtiEntryDto>> GetLineLoadedMaterialAsync(string lineCode) =>
            (await _gtt.GetLoadedEtisByLineAsync(lineCode).ConfigureAwait(false))
            .Select(item => new PointOfUseEtiEntryDto(item.PointOfUseCode, item.EtiNo, item.ComponentNo, item.UtcEffectiveTime.ToLocalTime()));
    }
}
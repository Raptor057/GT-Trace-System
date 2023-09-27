using GT.Trace.App.UseCases.Lines.UpdateGama;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Gateways
{
    internal class SqlUpdateGamaGateway:IUpdateBomLineGateway
    {
        private readonly GttDao _gtt;
        private readonly BomDao _bom;

        public SqlUpdateGamaGateway(GttDao gtt, BomDao bom)
        {
            _gtt = gtt;
            _bom=bom;
        }

        //public async Task<IEnumerable<UpdateBomLineDto>> GetLineandPartnofromPointOfUse(string PointOfUse)=>
        //    await _gtt.GetLineandPartnofromPointOfUseAsync(PointOfUse).ConfigureAwait(false);

        public async Task <int> UpdateGamaTrazab(string partNo, string lineCode)
            => await _bom.UpdateGamaAsync(partNo, lineCode).ConfigureAwait(false);

        public async Task<int> UpdateGamaGtt(string partNo, string lineCode)
            => await _gtt.UpdateGamaAsync(partNo, lineCode).ConfigureAwait(false);
    }
}

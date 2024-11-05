
using GT.Trace.App.UseCases.Lines.DeleteBomLine;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Gateways
{
    internal class SqlDeleteBomLineGateway : IDeleteBomLineGateway
    {
        private readonly GttDao _gtt;
        private readonly BomDao _bom;

        public SqlDeleteBomLineGateway(GttDao gtt, BomDao bom)
        {
            _gtt = gtt;
            _bom = bom;
        }

        public Task DeleteGamaGTTAsync(string partNo, string lineCode)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteGamaTrazabAsync(string partNo, string lineCode)=>
            await _bom.DeleteGama(partNo, lineCode).ConfigureAwait(false);
    }
}

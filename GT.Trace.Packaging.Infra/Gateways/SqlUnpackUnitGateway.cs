using GT.Trace.Packaging.App.UseCases.UnpackUnit;
using GT.Trace.Packaging.Infra.DataSources;

namespace GT.Trace.Packaging.Infra.Gateways
{
    public class SqlUnpackUnitGateway : IUnpackUnitGateway
    {
        private readonly GttSqlDB _gtt;

        public SqlUnpackUnitGateway(GttSqlDB gtt) 
        { 
            _gtt=gtt; 
        }

        public async Task UnpackedUnitAsync(string lineName, long unitID, string workOrderCode, string lineCode)=>
            await _gtt.UnpackedUnitAsync(lineName, unitID, workOrderCode, lineCode).ConfigureAwait(false);
    }
}

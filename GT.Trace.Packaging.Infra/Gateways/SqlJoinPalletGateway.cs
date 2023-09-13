using GT.Trace.Packaging.App.UseCases.JoinLinePallet;
using GT.Trace.Packaging.Infra.DataSources;

namespace GT.Trace.Packaging.Infra.Gateways
{
    internal class SqlJoinPalletGateway : IJoinLinePalletGateway
    {
        private readonly GttSqlDB _gtt;

        public SqlJoinPalletGateway(GttSqlDB gtt)
        {
            _gtt=gtt;
        }

        public async Task AddJoinPalletAsync(long UnitID, string PalletQR, string LineCode)=>
            await _gtt.AddPalletQR(UnitID, PalletQR,LineCode).ConfigureAwait(false);

        public async Task<int> PalletRegisteredInformationAsync(long UnitID)=>
            await _gtt.PalletRegisteredInformation(UnitID).ConfigureAwait(false);
    }
}

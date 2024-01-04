using GT.Trace.Packaging.App.UseCases.JoinFramelessMotors;
using GT.Trace.Packaging.Infra.DataSources;

namespace GT.Trace.Packaging.Infra.Gateways
{
    internal class SqlJoinFramelessMotorsGateway : IJoinFramelessMotorsGateway
    {
        private readonly GttSqlDB _gtt;

        public SqlJoinFramelessMotorsGateway(GttSqlDB gtt)
        {
            _gtt = gtt;
        }
        public async Task AddJoinFramelessMotorsAsync(long unitID, string componentID, string lineCode, string partNo) => await _gtt.AddJoinFramelessMotors(unitID,componentID,lineCode, partNo).ConfigureAwait(false);
        public async Task DelJoinFramelessMotorsAsync(long unitID, string componentID)=>await _gtt.DelJoinFramelessMotors(unitID, componentID).ConfigureAwait(false);
        public async Task<int> FramelessRegisteredInformationUnitIDAsync(long unitID) => await _gtt.FramelessRegisteredInformationUnitID(unitID).ConfigureAwait(false);
        public async Task<int> FramelessRegisteredInformationComponentIDAsync(string componentID) => await _gtt.FramelessRegisteredInformationComponentID(componentID).ConfigureAwait(false);
        public async Task<int> FramelessRegisteredInformationAsync(long unitID, string componentID) => await _gtt.FramelessRegisteredInformation(unitID,componentID).ConfigureAwait(false);


    }
}

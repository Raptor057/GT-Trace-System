using GT.Trace.BomSnapShot.App.Gateways;
using GT.Trace.BomSnapShot.Infra.DataSources;

namespace GT.Trace.BomSnapShot.Infra.Gateways
{
    internal sealed class SqlSeqSnapshotIDGateway : IGetSeqSnapshotIDGateways
    {
        private readonly GttSqlDB _gtt;

        public SqlSeqSnapshotIDGateway(GttSqlDB gtt)
        {
            _gtt=gtt;
        }

        public async Task<string> GetSeqSnapshotIDByLineCodeandPartNo(string lineCode, string partNo)
        {
            var GetSeqSnapshotID = await _gtt.GetSeqSnapshotIDByLineCodeandPartNoAsync(lineCode, partNo).ConfigureAwait(false);

            return GetSeqSnapshotID;
        }

        //async Task<long> IGetSeqSnapshotIDGateways.GetSeqSnapshotIDByLineCodeandPartNo(string lineCode, string partNo)
        //{
        //    var GetSeqSnapshotID = await _gtt.GetSeqSnapshotIDByLineCodeandPartNoAsync(lineCode, partNo).ConfigureAwait(false);

        //    return GetSeqSnapshotID;
        //}
    }
}


using GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot;
using GT.Trace.BomSnapShot.Infra.DataSources;
using GT.Trace.BomSnapShot.Infra.DataSources.Entities;

namespace GT.Trace.BomSnapShot.Infra.Gateways
{
    internal class SqlSaveSnapshotGateway : ISaveSnapshotGateway
    {
        private readonly GttSqlDB _gtt;
        private readonly AppsSqlDB _apps;
        private readonly TrazaSqlDB _traza;

        public SqlSaveSnapshotGateway(TrazaSqlDB traza, AppsSqlDB apps, GttSqlDB gtt)
        {
            _gtt=gtt;
            _apps=apps;
            _traza=traza;
        }

        //Esto guarda el snapshot
        public async Task<string> SaveSnapshotAsync(string pointOfUseCode)
        {


            await _gtt.SaveSnapShotAsync(pointOfUseCode).ConfigureAwait(false);
            return ("Save SnapShot OK");
        }
            
    }
}

using GT.Trace.Common.Infra;

namespace GT.Trace.Infra.Daos
{
    internal class StationDao : BaseDao
    {
        public StationDao(ITrazaSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<bool> SetStationBlockedStatusByLineAsync(bool isBlocked, string lineName, string processName) =>
            await Connection.ExecuteAsync("UPDATE pcmx SET is_blocked=@isBlocked WHERE LINE=@lineName AND PROCESSNAME=@processName;", new { isBlocked, lineName, processName })
            .ConfigureAwait(false) > 0;
    }
}
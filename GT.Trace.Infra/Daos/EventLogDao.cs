using GT.Trace.Common.Infra;
using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.Daos
{
    internal class EventLogDao : BaseDao
    {
        public EventLogDao(ITrazaSqlDBConnection connection)
            : base(connection)
        { }

        public async Task<bool> AddEventLogAsync(Tbl_events_log item) =>
            await Connection.ExecuteAsync("INSERT INTO tbl_events_log (error_msg,categoria,fecha,PCNAME) VALUES(@error_msg,@categoria,@fecha,@PCNAME);", item)
            .ConfigureAwait(false) > 0;
    }
}
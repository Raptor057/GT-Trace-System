using GT.Trace.Packaging.App.UseCases.SetStationBlocked;
using GT.Trace.Packaging.Infra.DataSources;
using GT.Trace.Packaging.Infra.DataSources.Entities;
namespace GT.Trace.Packaging.Infra.Gateways
{
    internal class SqlSetStationBlockedRepository : ISetStationBlockedGateway
    {
        private readonly DapperSqlDbConnection _con;

        public SqlSetStationBlockedRepository(ConfigurationSqlDbConnection<TrazaSqlDB> con)
        {
            _con = con;
        }

        public async Task StationBlocked(string is_blocked, string lineName)
        {
            await _con.ExecuteScalarAsync<pcmx>("UPDATE pcmx SET is_blocked = @is_blocked WHERE LINE LIKE('%'+@lineName);", new { is_blocked, lineName }).ConfigureAwait(false);
        }
    }
}

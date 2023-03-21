using GT.Trace.Packaging.Infra.DataSources.Entities;

namespace GT.Trace.Packaging.Infra.DataSources
{
    public class CegidSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public CegidSqlDB(ConfigurationSqlDbConnection<CegidSqlDB> con)
        {
            _con = con;
        }

        public async Task<REFEXT> GetRefExtAsync(string partNo, string revision, int clientCode) =>
            await _con.QuerySingleAsync<REFEXT>(
                "SELECT * FROM dbo.REFEXT WHERE RFKTSOC = 300 AND RTRIM(RFKTCODART) = @partNo AND RTRIM(RFKTCOMART) = @revision AND RTRIM(RFKTCODE) = @clientCode;",
                new
                {
                    partNo,
                    revision,
                    clientCode
                }).ConfigureAwait(false);

        public async Task<UARTICLE> GetUarticleAsync(string partNo, string revision) =>
            await _con.QuerySingleAsync<UARTICLE>(
                "SELECT * FROM dbo.UARTICLE WHERE ARKTSOC=300 AND RTRIM(ARKTCODART)=@partNo AND RTRIM(ARKTCOMART) = @revision",
                new { partNo, revision }).ConfigureAwait(false);

        public async Task<ARTICLE> GetArticleAsync(string partNo, string revision) =>
                    await _con.QuerySingleAsync<ARTICLE>(
                        "SELECT * FROM dbo.ARTICLE WHERE ARKTSOC = 300 AND RTRIM(ARKTCODART) = @partNo AND RTRIM(ARKTCOMART) = @revision;",
                        new { partNo, revision }).ConfigureAwait(false);

        public async Task<CLIENT> GetClientAsync(int clientCode) =>
                    await _con.QuerySingleAsync<CLIENT>(
                        "SELECT * FROM dbo.CLIENT WHERE CLKTSOC = 300 AND RTRIM(CLKTCODE) = @clientCode;",
                        new { clientCode }).ConfigureAwait(false);
    }
}
using GT.Trace.Packaging.App.UseCases.Auth;
using GT.Trace.Packaging.Infra.DataSources;

namespace GT.Trace.Packaging.Infra.Daos.Auth
{
    internal record UsersSqlDao(ConfigurationSqlDbConnection<AppsSqlDB> Connection) : IUsersDao
    {
        public async Task<bool> GetPasswordIsFromEnabledSupervisorUser(string password)
        {
            return await Connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.apps_users WHERE is_supervisor = 1 AND active = 1 AND password = @password;", new { password }).ConfigureAwait(false) > 0;
        }
    }
}
using GT.Trace.EZ2000.Packaging.App.UseCases.Auth;
using GT.Trace.EZ2000.Packaging.Infra.DataSources;

namespace GT.Trace.EZ2000.Packaging.Infra.Daos.Auth
{
    internal record UsersSqlDao(ConfigurationSqlDbConnection<AppsSqlDB> Connection) : IUsersDao
    {
        public async Task<bool> IsSupervisorUser(string password)
        {
            return await Connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.apps_users WHERE is_supervisor = 1 AND active = 1 AND password = @password;", new { password }).ConfigureAwait(false) > 0;
        }
    }
}
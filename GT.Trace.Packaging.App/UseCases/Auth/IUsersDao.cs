namespace GT.Trace.Packaging.App.UseCases.Auth
{
    public interface IUsersDao
    {
        Task<bool> GetPasswordIsFromEnabledSupervisorUser(string password);
    }
}
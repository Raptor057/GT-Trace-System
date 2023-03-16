namespace GT.Trace.EZ2000.Packaging.App.UseCases.Auth
{
    public interface IUsersDao
    {
        Task<bool> IsSupervisorUser(string password);
    }
}

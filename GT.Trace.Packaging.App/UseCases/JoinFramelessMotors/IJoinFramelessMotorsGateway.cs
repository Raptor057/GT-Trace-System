namespace GT.Trace.Packaging.App.UseCases.JoinFramelessMotors
{
    public interface IJoinFramelessMotorsGateway
    {
        Task AddJoinFramelessMotorsAsync(long unitID, string componentID, string lineCode, string partNo);
        Task DelJoinFramelessMotorsAsync(long unitID, string componentID);
        Task <int> FramelessRegisteredInformationUnitIDAsync(long unitID);
        Task<int> FramelessRegisteredInformationComponentIDAsync(string componentID);
        Task<int> FramelessRegisteredInformationAsync(long unitID, string componentID);
    }
}

namespace GT.Trace.TraceabilityLegacyIntegration.App.UseCases.Assembly.Utilities.MasterCancellation
{
    public interface IMasterCancellationGateway
    {
        Task<bool> CountMasterByIdAsync(long MasterID);
        Task MasterCancellationAsync(long MasterID);


    }
}

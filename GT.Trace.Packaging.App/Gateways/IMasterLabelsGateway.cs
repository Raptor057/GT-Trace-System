namespace GT.Trace.Packaging.App.Gateways
{
    public interface IMasterLabelsGateway
    {
        Task<long?> GetLastMasterFolioByLineAsync(string lineName);
    }
}
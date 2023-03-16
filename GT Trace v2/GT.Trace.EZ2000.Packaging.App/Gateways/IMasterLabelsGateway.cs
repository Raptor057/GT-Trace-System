namespace GT.Trace.EZ2000.Packaging.App.Gateways
{
    public interface IMasterLabelsGateway
    {
        Task<long?> GetLastMasterFolioByLineAsync(string lineName);
    }
}

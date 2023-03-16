namespace GT.Trace.Packaging.UI.PackagingWebApi.Hubs
{
    public interface IEventsHub
    {
        Task UnitPicked(string lineCode, long unitID);

        Task UnitPacked(string lineCode, long unitID, string workOrderCode);

        Task UnitUnpacked(string lineCode, long unitID);

        Task PalletComplete(string lineCode, string jsonData);

        Task ContainerComplete(string lineCode, string jsonData);

        Task PrintWip(string lineCode, string jsonData);
    }
}
namespace GT.Trace.UI.MaterialLoadingWebApi.Hubs
{
    public interface IPointsOfUseHub
    {
        Task EtiCreated(string lineCode, long etiID, string componentNo, string revision, string compDescription, int quantity, DateTime utcTimeStamp);
    }
}
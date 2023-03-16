namespace GT.Trace.Packaging.App.UseCases.SetLineHeadcount
{
    public interface ILineHeadcountGateway
    {
        Task SetCurrentShiftWorkOrderHeadcountAsync(string lineCode, string workOrderCode, int headcount);
    }
}
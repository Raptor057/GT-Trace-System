namespace GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseLines
{
    public interface IFetchPointOfUseLinesRepository
    {
        Task<string[]> FetchPointOfUseLinesAsync(string pointOfUseCode);
    }
}
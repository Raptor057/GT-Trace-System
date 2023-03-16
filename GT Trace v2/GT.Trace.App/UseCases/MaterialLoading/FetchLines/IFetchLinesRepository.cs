namespace GT.Trace.App.UseCases.MaterialLoading.FetchLines
{
    public interface IFetchLinesRepository
    {
        Task<IEnumerable<LineDto>> FetchLinesAsync();
    }
}
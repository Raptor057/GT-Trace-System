namespace GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis
{
    public interface IFetchPointOfUseEtisRepository
    {
        Task<IEnumerable<PointOfUseEtiDto>> FetchPointOfUseEtisAsync(string pointOfUseCode);
    }
}
namespace GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis
{
    public sealed record FetchPointOfUseEtisResponse(IEnumerable<PointOfUseEtiDto> Etis);
}
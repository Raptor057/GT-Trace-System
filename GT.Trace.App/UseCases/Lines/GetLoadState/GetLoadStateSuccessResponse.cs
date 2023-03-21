namespace GT.Trace.App.UseCases.Lines.GetLoadState
{
    public sealed record GetLoadStateSuccessResponse(IDictionary<GamaEntryDto, IEnumerable<PointOfUseEtiEntryDto>> State) : GetLoadStateResponse;
}
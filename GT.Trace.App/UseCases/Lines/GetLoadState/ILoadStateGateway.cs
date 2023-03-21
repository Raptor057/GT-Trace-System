namespace GT.Trace.App.UseCases.Lines.GetLoadState
{
    public interface ILoadStateGateway
    {
        Task<IEnumerable<PointOfUseEtiEntryDto>> GetLineLoadedMaterialAsync(string lineCode);

        Task<IEnumerable<GamaEntryDto>> GetGamaAsync(string partNo, string revision);
    }
}
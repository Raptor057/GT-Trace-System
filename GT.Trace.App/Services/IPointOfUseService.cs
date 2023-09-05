namespace GT.Trace.App.Services
{
    public interface IPointOfUseService
    {
        Task<bool> LoadMaterialAsync(string partNo, string workOrderCode, string etiNo, string pointOfUseCode, string componentNo);
        Task<bool> LoadMaterialAsync(string lineCode, string etiInput, string pointOfUseCode);
    }
}
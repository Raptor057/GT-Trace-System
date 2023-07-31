namespace GT.Trace.App.Services
{
    public interface IPointOfUseService
    {
        Task<bool> LoadMaterialAsync(string partNo, string workOrderCode, string etiNo, string pointOfUseCode, string componentNo);

        //NOTE: ESTO PERTENECE A SUB ENSAMBLE
        Task<bool> LoadMaterialAsync(string lineCode, string etiInput, string pointOfUseCode);
    }
}
namespace GT.Trace.App.Services
{
    public interface ICegidRadioService
    {
        Task<string> GenerateFabricationControlFileAsync(string? partNo, string? revision, string workOrderCode, int? quantity, long? etiID);
    }
}
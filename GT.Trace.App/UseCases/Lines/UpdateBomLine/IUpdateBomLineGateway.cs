namespace GT.Trace.App.UseCases.Lines.UpdateGama
{
    public interface IUpdateBomLineGateway
    {
        Task<int> UpdateGamaTrazab(string partNo, string lineCode);
        Task<int> UpdateGamaGtt(string partNo, string lineCode);
    }
}

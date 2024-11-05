namespace GT.Trace.App.UseCases.Lines.DeleteBomLine
{
    public interface IDeleteBomLineGateway
    {
        Task DeleteGamaTrazabAsync(string partNo, string lineCode);
        Task DeleteGamaGTTAsync(string partNo, string lineCode);
    }
}

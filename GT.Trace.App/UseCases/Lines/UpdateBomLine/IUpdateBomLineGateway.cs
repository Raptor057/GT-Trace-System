namespace GT.Trace.App.UseCases.Lines.UpdateGama
{
    public interface IUpdateBomLineGateway
    {
        //Task<IEnumerable<UpdateBomLineDto>> GetLineandPartnofromPointOfUse(string PointOfUse);
        Task<int> UpdateGamaTrazab(string partNo, string lineCode);
        Task<int> UpdateGamaGtt(string partNo, string lineCode);
    }
}

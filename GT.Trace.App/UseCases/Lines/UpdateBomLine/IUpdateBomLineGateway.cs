namespace GT.Trace.App.UseCases.Lines.UpdateGama
{
    public interface IUpdateBomLineGateway
    {
        //Task<IEnumerable<UpdateBomLineDto>> GetLineandPartnofromPointOfUse(string PointOfUse);
        Task<int> UpdateGama(string partNo, string lineCode);
    }
}

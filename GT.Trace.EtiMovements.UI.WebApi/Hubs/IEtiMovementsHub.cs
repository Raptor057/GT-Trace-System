namespace GT.Trace.EtiMovements.UI.WebApi.Hubs
{
    public interface IEtiMovementsHub
    {
        Task EtiLoaded(string lineCode, string etiNo, string componentNo, string pointOfUseCode);

        Task EtiUnloaded(string lineCode, string etiNo, string componentNo, string pointOfUseCode);

        Task EtiUsed(string lineCode, string etiNo, string componentNo, string pointOfUseCode);

        Task EtiReturned(string lineCode, string etiNo, string partNo, string componentNo, string pointOfUseCode, string operatorNo, DateTime utcTimeStamp);

        //Task UpdateEtiTraza(string etiNo); //<--Nuevo
    }
}
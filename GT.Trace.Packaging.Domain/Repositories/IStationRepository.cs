namespace GT.Trace.Packaging.Domain.Repositories
{
    using Domain.Entities;

    public interface IStationRepository
    {
        Task<Station> GetStationByHostnameAsync(string hostname, string? lineCode, int? palletSize, int? containerSize, string? poNumber);

        Task SaveAsync(Station station);

        Task<long?> GetLatestMasterLabelFolioByLineAsync(string lineName);
        Task <string?> GetOrigenByCegid(string partNo, string partRev);

        #region EZ
        /*Nuevo para EZ 
         Candados que falta en el Sistema de Traza
        Correo de Fabien Gurrier Lun 2023-12-04 7:53 AM
        */

        Task<string?> GetPartNoAsync(string lineCode);

        Task<bool> GetFuncionalTestResultAsync(long unitID);

        Task<bool> GetProcessHistoryAsync(long unitID);
        #endregion

    }
}
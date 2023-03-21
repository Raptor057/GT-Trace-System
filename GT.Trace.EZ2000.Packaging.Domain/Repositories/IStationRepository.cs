using GT.Trace.EZ2000.Packaging.Domain.Entities;

namespace GT.Trace.EZ2000.Packaging.Domain.Repositories
{
    public interface IStationRepository
    {
        Task<Station> GetStationByHostnameAsync(string hostname, string? lineCode, int? palletSize, int? containerSize, string? PoNumber);
        Task SaveAsync(Station station);
        Task<long?> GetLatestMasterLabelFolioByLineAsync(string lineName);
    }
}

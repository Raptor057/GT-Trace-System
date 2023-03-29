namespace GT.Trace.Packaging.Domain.Repositories
{
    using Domain.Entities;

    public interface IStationRepository
    {
        Task<Station> GetStationByHostnameAsync(string hostname, string? lineCode, int? palletSize, int? containerSize, string? poNumber);

        Task SaveAsync(Station station);

        Task<long?> GetLatestMasterLabelFolioByLineAsync(string lineName);
        Task <string?> GetOrigenByCegid(string partNo, string partRev);
    }
}
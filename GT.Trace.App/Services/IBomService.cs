using GT.Trace.App.Dtos;

namespace GT.Trace.App.Services
{
    public interface IBomService
    {
        Task<BomEntryDto?> GetBomEntryForComponentInLine(string partNo, string lineCode);
    }
}
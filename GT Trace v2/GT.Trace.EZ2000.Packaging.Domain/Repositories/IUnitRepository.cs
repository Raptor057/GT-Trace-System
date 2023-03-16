using GT.Trace.EZ2000.Packaging.Domain.Entities;

namespace GT.Trace.EZ2000.Packaging.Domain.Repositories
{
    public interface IUnitRepository
    {
        Task<Unit?> GetUnitByIDAsync(long id);
        Task<long> AddUnitAsync(string lineCode, int position, string serialCode, DateTime creationTime);
        Task<long?> GetUnitIDBySerialCodeAsync(string serialCode);
    }
}

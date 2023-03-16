namespace GT.Trace.Packaging.Domain.Repositories
{
    using Domain.Entities;

    public interface IUnitRepository
    {
        Task<Unit?> GetUnitByIDAsync(long id);

        Task<long> AddUnitAsync(string lineCode, int position, string serialCode, DateTime creationTime);

        Task<long?> GetUnitIDBySerialCodeAsync(string serialCode);
    }
}
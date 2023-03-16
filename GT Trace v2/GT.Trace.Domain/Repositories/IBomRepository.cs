using GT.Trace.Domain.Entities;

namespace GT.Trace.Domain.Repositories
{
    public interface IBomRepository
    {
        Task<IEnumerable<BomComponent>> FetchBomAsync(string partNo, string lineCode);
    }
}

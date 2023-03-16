using GT.Trace.Domain.Entities;

namespace GT.Trace.Domain.Repositories
{
    public interface ILineRepository
    {
        Task<Line> GetByCodeAsync(string lineCode, string? workOrderCode = null);

        Task SaveAsync(Line line);
    }
}
using GT.Trace.Common;
using GT.Trace.EtiMovements.Domain.Entities;

namespace GT.Trace.EtiMovements.Domain.Repositories
{
    public interface ILineRepository
    {
        Task<Result<Line>> GetAsync(string lineCode);

        Task SaveAsync(Line line);
    }
}
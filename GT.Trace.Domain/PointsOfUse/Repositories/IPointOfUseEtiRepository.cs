using GT.Trace.Common;
using GT.Trace.Domain.PointsOfUse.Entities;

namespace GT.Trace.Domain.PointsOfUse.Repositories
{
    public interface IPointOfUseEtiRepository
    {
        Task<Result<PointOfUseEti>> GetAsync(string etiNo, string partNo, string revision, string pointOfUseCode);

        Task SaveAsync(PointOfUseEti eti);
    }
}
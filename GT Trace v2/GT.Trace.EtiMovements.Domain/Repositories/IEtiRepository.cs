using GT.Trace.Common;
using GT.Trace.EtiMovements.Domain.Entities;

namespace GT.Trace.EtiMovements.Domain.Repositories
{
    public interface IEtiRepository
    {
        Task<Result<Eti>> TryGetAsync(string etiInput);
    }
}
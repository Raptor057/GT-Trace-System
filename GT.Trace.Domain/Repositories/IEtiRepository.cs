using GT.Trace.Domain.Entities;

namespace GT.Trace.Domain.Repositories
{
    public interface IEtiRepository
    {
        Task<Eti> TryGetEtiByIDAsync(long etiID, string etiNo);
    }
}
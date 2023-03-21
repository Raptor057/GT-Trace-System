using GT.Trace.Etis.Domain.Entities;

namespace GT.Trace.Etis.Domain.Repositories
{
    public interface IEtiRepository
    {
        Task<Eti> TryGetEtiByIDAsync(long etiID, string etiNo);
    }
}
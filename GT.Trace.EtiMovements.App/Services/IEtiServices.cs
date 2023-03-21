using GT.Trace.Common;
using GT.Trace.EtiMovements.App.Dtos;

namespace GT.Trace.EtiMovements.App.Services
{
    public interface IEtiServices
    {
        Task<Result<EtiInfoDto>> GetEtiAsync(string etiInput);
    }
}
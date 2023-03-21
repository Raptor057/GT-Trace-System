using GT.Trace.App.Dtos;
using GT.Trace.Common.Infra.HttpApi;

namespace GT.Trace.App.Services
{
    public interface IEtiService
    {
        Task<HttpApiJsonResponse<EtiInfoDto>> GetEtiInfoAsync(string scannerInput);
    }
}
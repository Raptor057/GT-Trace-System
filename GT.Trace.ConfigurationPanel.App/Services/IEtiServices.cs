using GT.Trace.Common;
using GT.Trace.ConfigurationPanel.App.Dtos;

namespace GT.Trace.ConfigurationPanel.App.Services
{
    public interface IEtiServices
    {
        Task<Result<EtiInfoDto>> GetEtiAsync(string etiInput);
    }
}

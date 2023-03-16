using GT.Trace.Common;
using GT.Trace.Common.Infra.HttpApi;
using GT.Trace.EtiMovements.App.Dtos;
using GT.Trace.EtiMovements.App.Services;
using Microsoft.Extensions.Configuration;

namespace GT.Trace.EtiMovements.Infra.Services
{
    internal class HttpEtiServices : IEtiServices
    {
        private static Lazy<HttpApiClient>? _client;

        public HttpEtiServices(IConfigurationRoot configuration)
        {
            _client = new(() => new(configuration.GetSection("HttpApi:HttpEtiServices").Value), true);
        }

        public async Task<Result<EtiInfoDto>> GetEtiAsync(string etiInput)
        {
            var response = await _client!.Value.PostJsonAsync<EtiInfoDto>("/api/info", etiInput).ConfigureAwait(false);
            if (!response.IsSuccess)
            {
                return Result.Fail<EtiInfoDto>(response.Message ?? "ERROR");
            }
            return Result.OK(response.Data!);
        }
    }
}
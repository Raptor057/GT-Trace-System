using GT.Trace.App.Dtos;
using GT.Trace.App.Services;
using GT.Trace.Common.Infra.HttpApi;

namespace GT.Trace.Infra.Services
{
    internal class EtiWebApiService : IEtiService
    {
        private readonly Lazy<HttpApiClient> _client;

        public EtiWebApiService()
        {
            _client = new(() => new("http://mxsrvapps/gtt/services/etis"), true);
        }

        //public async Task<HttpApiJsonResponse<EtiInfoDto>> GetEtiInfoAsync(string scannerInput)
        //{
        //    var response = await _client.Value.PostAsync<dynamic, EtiInfoDto>("/api/info", new { ScannerInput = scannerInput }).ConfigureAwait(false);
        //    if (response == null)
        //    {
        //        throw new InvalidOperationException("Ocurrió un error al intentar obtener respuesta del servicio de ETIs.");
        //    }
        //    return response;
        //}

        public async Task<HttpApiJsonResponse<EtiKeyDto>> ParseInfoAsync(string etiInput)
        {
            var response = await _client!.Value.GetJsonAsync<EtiKeyDto>($"/api/parse/{etiInput}").ConfigureAwait(false);
            if (response == null)
            {
                throw new InvalidOperationException("Ocurrió un error al intentar obtener respuesta del servicio de ETIs.");
            }
            return response;
        }

        public async Task<HttpApiJsonResponse<EtiInfoDto>> GetEtiInfoAsync(string etiInput)
        {
            var response = await _client!.Value.GetJsonAsync<EtiInfoDto>($"/api/info/{etiInput}").ConfigureAwait(false);
            if (response == null)
            {
                throw new InvalidOperationException("Ocurrió un error al intentar obtener respuesta del servicio de ETIs.");
            }
            return response;
        }

        public async Task<HttpApiJsonResponse<EtiInfoDto>> GetEtiInfoAsync(long etiID, string etiNo)
        {
            var response = await _client!.Value.GetJsonAsync<EtiInfoDto>($"/api/info/{etiID}/{etiNo}").ConfigureAwait(false);
            if (response == null)
            {
                throw new InvalidOperationException("Ocurrió un error al intentar obtener respuesta del servicio de ETIs.");
            }
            return response;
        }
    }
}
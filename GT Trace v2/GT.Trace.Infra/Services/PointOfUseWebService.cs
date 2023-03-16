using GT.Trace.App.Services;
using GT.Trace.Common.Infra.HttpApi;
using Microsoft.Extensions.Configuration;

namespace GT.Trace.Infra.Services
{
    internal class PointOfUseWebService : IPointOfUseService
    {
        private static Lazy<HttpApiClient>? _client;

        public PointOfUseWebService(IConfigurationRoot configuration)
        {
            _client = new(() => new(configuration.GetSection("HttpApi:PointOfUseWebService").Value), true);
        }

        public async Task<bool> LoadMaterialAsync(string lineCode, string etiInput, string pointOfUseCode)
        {
            await _client!.Value.PostAsync($"/api/lines/{lineCode}/etis", new { EtiInput = etiInput, PointOfUseCode = pointOfUseCode, IgnoreCapacity = true }).ConfigureAwait(false);
            return true;
        }

        public Task<bool> LoadMaterialAsync(string partNo, string workOrderCode, string etiNo, string pointOfUseCode, string componentNo)
        {
            throw new NotImplementedException();
        }
    }
}
using GT.Trace.Changeover.App.UseCases.ApplyChangeover;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace GT.Trace.Changeover.Infra.Services
{
    internal class HttpReturnLabelPrintingService : IReturnLabelPrintingService, IDisposable
    {
        private sealed record JsonResponse(string Message);

        private readonly Lazy<HttpClient> _httpClient = new();

        private readonly IConfigurationRoot _configuration;

        public HttpReturnLabelPrintingService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<string>> ExecuteAsync(string lineCode, string[] etis)
        {
            var errors = new List<string>();

            var uri = new Uri(string.Format(_configuration["HttpReturnLabelPrintingServiceUri"], lineCode));
            _httpClient.Value.BaseAddress = uri;
            foreach (var eti in etis)
            {
                var data = JsonConvert.SerializeObject(new { EtiInput = $"{eti}", IsReturn = true });
                var request = new HttpRequestMessage(HttpMethod.Delete, uri)
                {
                    Content = new StringContent(data, Encoding.UTF8, "application/json")
                };

                using var response = await _httpClient.Value.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(responseContent);
                    errors.Add(jsonResponse!.Message);
                }
            }
            return errors;
        }

        public void Dispose()
        {
            _httpClient.Value?.Dispose();
        }
    }
}
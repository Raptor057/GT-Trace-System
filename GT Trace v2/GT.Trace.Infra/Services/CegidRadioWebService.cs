using GT.Trace.App.Services;
using GT.Trace.Common.Infra.HttpApi;
using Microsoft.Extensions.Configuration;

namespace GT.Trace.Infra.Services
{
    internal class CegidRadioWebService : ICegidRadioService
    {
        private static Lazy<HttpApiClient>? _client;

        public CegidRadioWebService(IConfigurationRoot configuration)
        {
            _client = new(() => new(configuration.GetSection("HttpApi:CegidRadioService").Value), true);
        }

        public async Task<string> GenerateFabricationControlFileAsync(string? partNo, string? revision, string workOrderCode, int? quantity, long? etiID)
        {
            var response = await _client!.Value.PostAsync("/api/fabricationcontrol", new { PartNo = partNo, Revision = revision, WorkOrderCode = workOrderCode, Quantity = quantity, EtiID = etiID, DepoCode = "ENSAMB", OrderIsClosed = false }).ConfigureAwait(false);
            return response?.Message!;
        }
    }
}
using GT.Trace.Common;
using GT.Trace.Common.Infra.HttpApi;
using GT.Trace.EtiMovements.App.Dtos;
using GT.Trace.EtiMovements.App.Services;
using Microsoft.Extensions.Configuration;

namespace GT.Trace.EtiMovements.Infra.Services
{
    internal class HttpLineServices : ILineServices
    {
        private static Lazy<HttpApiClient>? _client;

        public HttpLineServices(IConfigurationRoot configuration)
        {
            _client = new(() => new(configuration.GetSection("HttpApi:HttpLineServices").Value ?? ""), true);
        }

        public async Task<Result<IEnumerable<BomComponentDto>>> GetBomAsync(string partNo, string lineCode)
        {
            var response = await _client!.Value.GetJsonAsync<IEnumerable<BomComponentDto>>($"/api/lines/{lineCode}/bom/{partNo}").ConfigureAwait(false);
            if (!response.IsSuccess)
            {
                return Result.Fail<IEnumerable<BomComponentDto>>(response.Message ?? "ERROR");
            }
            return Result.OK(response.Data!);
        }

        public async Task<Result<WorkOrderDto>> GetWorkOrderAsync(string lineCode)
        {
            var response = await _client!.Value.GetJsonAsync<WorkOrderDto>($"/api/lines/{lineCode}/workorder").ConfigureAwait(false);
            if (!response.IsSuccess)
            {
                return Result.Fail<WorkOrderDto>(response.Message ?? "ERROR");
            }
            return Result.OK(response.Data!);
        }
    }
}
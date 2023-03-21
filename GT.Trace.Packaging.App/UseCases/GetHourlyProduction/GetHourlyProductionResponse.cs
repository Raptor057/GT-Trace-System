using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.Dtos;
using GT.Trace.Packaging.App.Gateways;

namespace GT.Trace.Packaging.App.UseCases.GetHourlyProduction
{
    public record GetHourlyProductionResponse(IEnumerable<HourlyProductionItemDto> HourlyProduction) : IResponse;

    public sealed record GetHourlyProductionRequest(string LineCode) : IRequest<GetHourlyProductionResponse>;

    internal sealed class GetHourlyProductionHandler : IInteractor<GetHourlyProductionRequest, GetHourlyProductionResponse>
    {
        private readonly IHourlyProductionGateway _gateway;

        public GetHourlyProductionHandler(IHourlyProductionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<GetHourlyProductionResponse> Handle(GetHourlyProductionRequest request, CancellationToken cancellationToken)
        {
            return new GetHourlyProductionResponse(await _gateway.GetHourlyProductionByLineAsync(request.LineCode).ConfigureAwait(false));
        }
    }
}
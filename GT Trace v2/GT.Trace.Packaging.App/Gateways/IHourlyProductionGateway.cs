using GT.Trace.Packaging.App.Dtos;

namespace GT.Trace.Packaging.App.Gateways
{
    public interface IHourlyProductionGateway
    {
        Task<IEnumerable<HourlyProductionItemDto>> GetHourlyProductionByLineAsync(string lineCode);
    }
}
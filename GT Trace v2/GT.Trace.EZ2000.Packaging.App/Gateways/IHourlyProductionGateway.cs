using GT.Trace.EZ2000.Packaging.App.Dtos;

namespace GT.Trace.EZ2000.Packaging.App.Gateways
{
    public interface IHourlyProductionGateway
    {
        Task<IEnumerable<HourlyProductionItemDto>> GetHourlyProductionByLineAsync(string lineCode);
    }
}

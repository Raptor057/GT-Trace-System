using GT.Trace.Common;
using GT.Trace.EtiMovements.App.Dtos;

namespace GT.Trace.EtiMovements.App.Services
{
    public interface ILineServices
    {
        Task<Result<IEnumerable<BomComponentDto>>> GetBomAsync(string partNo, string lineCode);

        Task<Result<WorkOrderDto>> GetWorkOrderAsync(string lineCode);
    }
}
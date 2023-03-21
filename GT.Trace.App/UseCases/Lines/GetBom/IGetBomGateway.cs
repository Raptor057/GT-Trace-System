using GT.Trace.Common;

namespace GT.Trace.App.UseCases.Lines.GetBom
{
    public interface IGetBomGateway
    {
        Task<Result<IEnumerable<BomComponentDto>>> GetBomAsync(string partNo, string revision);
    }
}
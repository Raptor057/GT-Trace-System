using GT.Trace.App.UseCases.Lines.GetBom;
using GT.Trace.Common;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Gateways
{
    internal record SqlGetLineBomGateway(BomDao Bom)
        : IGetBomGateway
    {
        public async Task<Result<IEnumerable<BomComponentDto>>> GetBomAsync(string partNo, string revision)
        {
            var bom = await Bom.FetchBomAsync(partNo, revision).ConfigureAwait(false);

            return Result.OK(bom.Select(item => new BomComponentDto(item.PartNo, item.PartRev, item.PointOfUse, int.Parse(item.Capacity), item.CompNo, item.CompRev, (int)item.ReqQty)));
        }
    }
}
﻿using GT.Trace.App.UseCases.Lines.GetBom;
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

            //BUG: Aqui se hizo un cambio en el item.capacity debido a que si es nulo o vacio daba error, y aqui si detecta algo asi lo cambia por un 2
            //return Result.OK(bom.Select(item => new BomComponentDto(item.PartNo, item.PartRev, item.PointOfUse, int.Parse(item.Capacity), item.CompNo, item.CompRev, (int)item.ReqQty)));
            return Result.OK(bom.Select(item => new BomComponentDto(item.PartNo, item.PartRev, item.PointOfUse, int.TryParse(item.Capacity, out int capacity) ? capacity : 2, item.CompNo, item.CompRev, (int)item.ReqQty)));
        }
    }
}
using GT.Trace.Domain.Entities;
using GT.Trace.Domain.Repositories;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Repositories
{
    internal record SqlBomRepository(BomDao Bom) : IBomRepository
    {
        public async Task<IEnumerable<BomComponent>> FetchBomAsync(string partNo, string lineCode)
        {
            var bom = await Bom.FetchBomAsync(partNo, lineCode).ConfigureAwait(false);

            //return bom.Select(item => new BomComponent(item.PointOfUse, item.CompNo, item.CompRev, item.CompDesc, int.Parse(item.Capacity)));
            return bom.Select(item => new BomComponent(
            item.PointOfUse,
            item.CompNo,
            item.CompRev,
            item.CompDesc,
            string.IsNullOrEmpty(item.Capacity) ? 1 : int.Parse(item.Capacity)));
        }
    }
}
using GT.Trace.App.Dtos;
using GT.Trace.App.Services;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Services
{
    internal record SqlBomService(BomDao Bom) : IBomService
    {
        public async Task<BomEntryDto?> GetBomEntryForComponentInLine(string partNo, string lineCode)
        {
            var entry = await Bom.GetLineBomEntryByComponent(partNo, lineCode)
                .ConfigureAwait(false);
            if (entry == null)
            {
                return null;
            }
            return new BomEntryDto(entry.NOKTCOMPF, entry.NOKTCODPF, entry.NOCTCODECP, entry.NOCTCODOPE);
        }
    }
}
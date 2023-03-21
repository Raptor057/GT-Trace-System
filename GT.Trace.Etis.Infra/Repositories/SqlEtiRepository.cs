using GT.Trace.Etis.Domain.Entities;
using GT.Trace.Etis.Domain.Repositories;
using GT.Trace.Etis.Infra.Daos;

namespace GT.Trace.Etis.Infra.Repositories
{
    internal record SqlEtiRepository(EtiDao Etis, SubEtiDao SubEtis, MotorsEtiDao MotorsEti) : IEtiRepository
    {
        public async Task<Eti> TryGetEtiByIDAsync(long etiID, string etiNo)
        {
            if (string.IsNullOrWhiteSpace(etiNo) || etiNo.Length < 3)
                throw new InvalidOperationException($"Numero de ETI no es valido [{etiNo}].");

            if (Eti.CheckEtiIsSubAssembly(etiNo))
            {
                var subeti = await SubEtis.GetSubEtiByIDAsync(etiID).ConfigureAwait(false) ?? throw new InvalidOperationException($"Sub ensamble ETI#{etiID} no encontrado.");
                return Eti.Create(etiID, etiNo, subeti.NP1.Trim(), subeti.rev1.Trim(), $"{subeti.lot1}//{subeti.lot2}", true);
            }
            else if (Eti.CheckEtiIsAssembly(etiNo))
            {
                var eti = await Etis.TryGetEtiByIDAsync(etiID).ConfigureAwait(false) ?? throw new InvalidOperationException($"Ensamble ETI#{etiID} no encontrada.");
                return Eti.Create(etiID, etiNo, eti.part_number.Trim(), eti.rev.Trim(), eti.lot, !(eti.Blocked ?? false));
            }
            else if (Eti.CheckEtiIsMotorsSubAssembly(etiNo))
            {
                var eti = await MotorsEti.TryGetEtiByIDAsync(etiID).ConfigureAwait(false) ?? throw new InvalidOperationException($"Ensamble ETI#{etiID} no encontrada.");
                var creationTime = eti.UtcCreationTime.ToLocalTime();
                var lotNo = $"{creationTime.Year}{creationTime.DayOfYear:000}";
                return Eti.Create(etiID, etiNo, eti.ComponentNo, eti.Revision, lotNo, false);
            }
            else
            {
                throw new InvalidOperationException($"ETI no valida [{etiNo}] para sub ensamble o ensamble.");
            }
        }
    }
}
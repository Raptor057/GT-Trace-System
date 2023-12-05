using GT.Trace.Domain.Entities;
using GT.Trace.Domain.Repositories;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Repositories
{
    internal record SqlEtiRepository(EtiDao Etis, SubEtiDao SubEtis, PointOfUseDao PointsOfUse, SubAssemblyDao SubAssembly) : IEtiRepository
    {
        public async Task<Eti> TryGetEtiByIDAsync(long etiID, string etiNo)
        {
            if (string.IsNullOrWhiteSpace(etiNo) || etiNo.Length < 3)
                throw new InvalidOperationException($"Numero de ETI no es valido [{etiNo}].");

            EtiStatus? etiStatus = null;
            var info = await PointsOfUse.GetEtiInfoAsync(etiNo).ConfigureAwait(false);
            if (info != null)
            {
                etiStatus = new EtiStatus(info.PointOfUseCode, info.UtcEffectiveTime.ToLocalTime(), info.UtcUsageTime?.ToLocalTime(), info.UtcExpirationTime?.ToLocalTime(), info.IsDepleted);
            }

            if (Eti.CheckEtiIsSubAssembly(etiNo))
            {
                var subeti = await SubEtis.GetSubEtiByIDAsync(etiID).ConfigureAwait(false) ?? throw new InvalidOperationException($"Sub ensamble ETI#{etiID} no encontrado.");
                return Eti.Create(etiID, etiNo, Part.Create(subeti.NP1, new Revision(subeti.rev1), null, null), $"{subeti.lot1}//{subeti.lot2}", true, etiStatus);
            }
            else if (Eti.CheckEtiIsAssembly(etiNo))
            {
                var eti = await Etis.TryGetEtiByIDAsync(etiID).ConfigureAwait(false) ?? throw new InvalidOperationException($"Ensamble ETI#{etiID} no encontrada.");
                return Eti.Create(etiID, etiNo, Part.Create(eti.part_number, new Revision(eti.rev)), eti.lot, !(eti.Blocked ?? false), etiStatus);
            }
            #region Esto se agrego el 08/04/2023 para arreglar la carga de sub ensambles con SA
            else if (Eti.CheckEtiIsMotorsSubAssembly(etiNo))
            {
                var eti = await SubAssembly.TryGetEtiByIDAsync(etiID).ConfigureAwait(false) ?? throw new InvalidOperationException($"Ensamble ETI#{etiID} no encontrada.");
                //var creationTime = eti.UtcCreationTime.ToLocalTime();
                //var lotNo = $"{creationTime.Year}{creationTime.DayOfYear:000}";
                //return Eti.Create(etiID, etiNo, Part.Create(eti.ComponentNo, new Revision(eti.Revision)), eti.WorkOrderCode, true/*!(eti.IsDisabled ?? false)*/, etiStatus);
                return Eti.Create(etiID, etiNo, Part.Create(eti.ComponentNo ?? "", new Revision(eti.Revision ?? ""), null, null), $"{eti.WorkOrderCode}", true, etiStatus);
            }
            #endregion
            else
            {
                throw new InvalidOperationException($"ETI no valida [{etiNo}] para sub ensamble o ensamble.");
            }
        }
    }
}
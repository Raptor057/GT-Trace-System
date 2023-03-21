using GT.Trace.Common;
using GT.Trace.EtiMovements.App.Services;
using GT.Trace.EtiMovements.Domain.Entities;
using GT.Trace.EtiMovements.Domain.Repositories;
using GT.Trace.EtiMovements.Infra.Daos;

namespace GT.Trace.EtiMovements.Infra.Repositories
{
    internal sealed class SqlEtiRepository
        : IEtiRepository
    {
        private readonly PointOfUseEtiDao _pointsOfUse;

        private readonly IEtiServices _etis;

        public SqlEtiRepository(PointOfUseEtiDao pointsOfUse, IEtiServices etis)
        {
            _pointsOfUse = pointsOfUse;
            _etis = etis;
        }

        public async Task<Result<Eti>> TryGetAsync(string etiInput)
        {
            var getEtiResult = await _etis.GetEtiAsync(etiInput).ConfigureAwait(false);
            if (getEtiResult is IFailure failure)
            {
                return Result.Fail<Eti>(failure.Message);
            }

            var etiInfo = (getEtiResult as ISuccess<App.Dtos.EtiInfoDto>)?.Data ?? throw new NullReferenceException();

            if (!Eti.CanCreate(etiInfo.EtiNo, etiInfo.ComponentNo, etiInfo.LotNo, Revision.New(etiInfo.Revision), out var errors))
            {
                return Result.Fail<Eti>(errors.ToString());
            }

            var movement = await _pointsOfUse.GetEtiLastMovementAsync(etiInfo.EtiNo).ConfigureAwait(false);
            if (movement != null && !EtiMovement.CanCreate(movement.PointOfUseCode, movement.EtiNo, movement.ComponentNo, out errors))
            {
                return Result.Fail<Eti>(errors.ToString());
            }

            bool etiIsShared = false;
            if (movement != null)
            {
                etiIsShared = await _pointsOfUse.CountLinesSharingPointOfUseAsync(movement.PointOfUseCode, etiInfo.ComponentNo).ConfigureAwait(false) > 1;
            }

            return Result.OK(Eti.Create(
                etiInfo.EtiNo,
                etiInfo.ComponentNo,
                etiInfo.LotNo,
                Revision.New(etiInfo.Revision),
                etiIsShared,
                movement == null
                    ? null
                    : EtiMovement.Create(
                        movement.PointOfUseCode,
                        movement.EtiNo,
                        movement.ComponentNo,
                        movement.UtcEffectiveTime,
                        movement.UtcUsageTime,
                        movement.UtcExpirationTime,
                        movement.IsDepleted)));
        }
    }
}
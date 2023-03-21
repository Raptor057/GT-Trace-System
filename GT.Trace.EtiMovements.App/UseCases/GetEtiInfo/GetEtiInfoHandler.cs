using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.Domain.Entities;
using GT.Trace.EtiMovements.Domain.Repositories;

namespace GT.Trace.EtiMovements.App.UseCases.GetEtiInfo
{
    internal sealed class GetEtiInfoHandler
        : ResultInteractorBase<GetEtiInfoRequest, GetEtiInfoResponse>
    {
        public readonly ILineRepository _lines;

        public readonly IEtiRepository _etis;

        public GetEtiInfoHandler(ILineRepository lines, IEtiRepository etis)
        {
            _lines = lines;
            _etis = etis;
        }

        public override async Task<Result<GetEtiInfoResponse>> Handle(GetEtiInfoRequest request, CancellationToken cancellationToken)
        {
            var getEtiResult = await _etis.TryGetAsync(request.EtiInput ?? "").ConfigureAwait(false);
            if (getEtiResult is IFailure getEtiFailure)
            {
                return Fail(getEtiFailure.Message);
            }

            var eti = (getEtiResult as ISuccess<Eti>)?.Data ?? throw new NullReferenceException();

            string? status = null;
            if (eti.IsActive)
            {
                status = $"Material en uso desde el día {eti.LastMovement!.UsageTime:dd/MMM/yy a la\\s HH:mm:ss} hrs. en el túnel \"{eti.LastMovement!.PointOfUseCode}\".";
            }
            else if (eti.IsLoaded)
            {
                status = $"Material cargado desde el día {eti.LastMovement!.StartTime:dd/MMM/yy a la\\s HH:mm:ss} hrs. en el túnel \"{eti.LastMovement!.PointOfUseCode}\".";
            }
            else if (eti.LastMovement?.IsDepleted ?? false)
            {
                status = $"Material consumido desde el día {eti.LastMovement!.EndTime:dd/MMM/yy a la\\s HH:mm:ss} hrs. en el túnel \"{eti.LastMovement!.PointOfUseCode}\".";
            }

            return OK(new GetEtiInfoResponse(eti.Number, eti.ComponentNo, eti.Revision.OriginalValue, status));
        }
    }
}
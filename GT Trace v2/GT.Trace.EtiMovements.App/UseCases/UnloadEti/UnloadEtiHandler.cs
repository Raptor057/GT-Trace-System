using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.Domain.Entities;
using GT.Trace.EtiMovements.Domain.Repositories;

namespace GT.Trace.EtiMovements.App.UseCases.UnloadEti
{
    internal sealed class UnloadEtiHandler
        : ResultInteractorBase<UnloadEtiRequest, UnloadEtiResponse>
    {
        public readonly ILineRepository _lines;

        public readonly IEtiRepository _etis;

        public UnloadEtiHandler(ILineRepository lines, IEtiRepository etis)
        {
            _lines = lines;
            _etis = etis;
        }

        public override async Task<Result<UnloadEtiResponse>> Handle(UnloadEtiRequest request, CancellationToken cancellationToken)
        {
            var getEtiResult = await _etis.TryGetAsync(request.EtiInput).ConfigureAwait(false);
            if (getEtiResult is IFailure getEtiFailure)
            {
                return Fail(getEtiFailure.Message);
            }

            var eti = (getEtiResult as ISuccess<Eti>)?.Data ?? throw new NullReferenceException();

            if (!eti!.CanUnload(out var errors))
            {
                return Fail(errors.ToString());
            }

            var getLineResult = await _lines.GetAsync(request.LineCode).ConfigureAwait(false);
            if (getLineResult is IFailure getLineFailure)
            {
                return Fail(getLineFailure.Message);
            }

            var line = (getLineResult as ISuccess<Line>)?.Data ?? throw new NullReferenceException();

            if (!line.CanUnloadEti(eti!, out errors))
            {
                return Fail(errors.ToString());
            }

            line.UnloadEti(eti!);

            await _lines.SaveAsync(line).ConfigureAwait(false);

            return OK(new UnloadEtiResponse(request.LineCode, eti.Number, eti.ComponentNo, eti.LastMovement!.PointOfUseCode));
        }
    }
}
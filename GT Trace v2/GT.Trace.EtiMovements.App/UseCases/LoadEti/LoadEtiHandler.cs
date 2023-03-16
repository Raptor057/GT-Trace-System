using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.Domain.Entities;
using GT.Trace.EtiMovements.Domain.Repositories;

namespace GT.Trace.EtiMovements.App.UseCases.LoadEti
{
    internal sealed class LoadEtiHandler
        : ResultInteractorBase<LoadEtiRequest, LoadEtiResponse>
    {
        public readonly ILineRepository _lines;

        public readonly IEtiRepository _etis;

        public LoadEtiHandler(ILineRepository lines, IEtiRepository etis)
        {
            _lines = lines;
            _etis = etis;
        }

        public override async Task<Result<LoadEtiResponse>> Handle(LoadEtiRequest request, CancellationToken cancellationToken)
        {
            var getEtiResult = await _etis.TryGetAsync(request.EtiInput).ConfigureAwait(false);
            if (getEtiResult is IFailure getEtiFailure)
            {
                return Fail(getEtiFailure.Message);
            }

            var eti = (getEtiResult as ISuccess<Eti>)?.Data ?? throw new NullReferenceException();

            if (!eti!.CanLoad(request.PointOfUseCode, out var errors))
            {
                return Fail(errors.ToString());
            }

            var getLineResult = await _lines.GetAsync(request.LineCode).ConfigureAwait(false);
            if (getLineResult is IFailure getLineFailure)
            {
                return Fail(getLineFailure.Message);
            }

            var line = (getLineResult as ISuccess<Line>)?.Data ?? throw new NullReferenceException();

            if (!line.CanLoadEti(request.PointOfUseCode, eti!, request.IgnoreCapacity, out errors))
            {
                return Fail(errors.ToString());
            }

            line.LoadEti(request.PointOfUseCode, eti!, request.IgnoreCapacity);

            await _lines.SaveAsync(line).ConfigureAwait(false);

            return OK(new LoadEtiResponse(request.LineCode, eti.Number, eti.ComponentNo, eti.LastMovement!.PointOfUseCode));
        }
    }
}
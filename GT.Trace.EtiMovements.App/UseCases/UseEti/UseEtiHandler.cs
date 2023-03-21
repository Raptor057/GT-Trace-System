using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.Domain.Repositories;

namespace GT.Trace.EtiMovements.App.UseCases.UseEti
{
    internal class UseEtiHandler
        : ResultInteractorBase<UseEtiRequest, UseEtiResponse>
    {
        public readonly ILineRepository _lines;

        public readonly IEtiRepository _etis;

        public UseEtiHandler(ILineRepository lines, IEtiRepository etis)
        {
            _lines = lines;
            _etis = etis;
        }

        public override async Task<Result<UseEtiResponse>> Handle(UseEtiRequest request, CancellationToken cancellationToken)
        {
            var getEtiResult = await _etis.TryGetAsync(request.EtiInput).ConfigureAwait(false);
            if (getEtiResult is IFailure getEtiFailure)
            {
                return Fail(getEtiFailure.Message);
            }

            var eti = (getEtiResult as ISuccess<Domain.Entities.Eti>)?.Data ?? throw new NullReferenceException();

            if (!eti!.CanUse(out var errors))
            {
                return Fail(errors.ToString());
            }

            var getLineResult = await _lines.GetAsync(request.LineCode).ConfigureAwait(false);
            if (getLineResult is IFailure getLineFailure)
            {
                return Fail(getLineFailure.Message);
            }

            var line = (getLineResult as ISuccess<Domain.Entities.Line>)?.Data ?? throw new NullReferenceException();

            if (!line.CanUseEti(eti!, out errors))
            {
                return Fail(errors.ToString());
            }

            line.UseEti(eti!);

            await _lines.SaveAsync(line).ConfigureAwait(false);

            return OK(new UseEtiResponse(request.LineCode, eti.Number, eti.ComponentNo, eti.LastMovement!.PointOfUseCode));
        }
    }
}
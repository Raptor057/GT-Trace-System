using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.Domain.Entities;
using GT.Trace.EtiMovements.Domain.Repositories;

namespace GT.Trace.EtiMovements.App.UseCases.ReturnEti
{
    internal sealed class ReturnEtiHandler
        : ResultInteractorBase<ReturnEtiRequest, ReturnEtiResponse>
    {
        public readonly ILineRepository _lines;

        public readonly IEtiRepository _etis;

        public ReturnEtiHandler(ILineRepository lines, IEtiRepository etis)
        {
            _lines = lines;
            _etis = etis;
        }

        public override async Task<Result<ReturnEtiResponse>> Handle(ReturnEtiRequest request, CancellationToken cancellationToken)
        {
            var getEtiResult = await _etis.TryGetAsync(request.EtiInput).ConfigureAwait(false);
            if (getEtiResult is IFailure getEtiFailure)
            {
                return Fail(getEtiFailure.Message);
            }

            var eti = (getEtiResult as ISuccess<Eti>)?.Data ?? throw new NullReferenceException();

            if (!eti!.CanReturn(request.IsChangeOver, out var errors))
            {
                return Fail(errors.ToString());
            }

            var getLineResult = await _lines.GetAsync(request.LineCode).ConfigureAwait(false);
            if (getLineResult is IFailure getLineFailure)
            {
                return Fail(getLineFailure.Message);
            }

            var line = (getLineResult as ISuccess<Line>)?.Data ?? throw new NullReferenceException();

            if (!line.CanReturnEti(eti!, request.IsChangeOver, out errors))
            {
                return Fail(errors.ToString());
            }

            line.ReturnEti(eti!, request.IsChangeOver);

            await _lines.SaveAsync(line).ConfigureAwait(false);

            return OK(new ReturnEtiResponse(request.LineCode, eti.Number, line.WorkOrder.PartNo, eti.ComponentNo, eti.LastMovement!.PointOfUseCode, "5766", DateTime.UtcNow));
        }
    }
}
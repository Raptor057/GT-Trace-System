using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetLine
{
    internal sealed class GetLineHandler : ResultInteractorBase<GetLineRequest, GetLineResponse>
    {
        private readonly IGetLineRepository _repo;

        public GetLineHandler(IGetLineRepository repo)
        {
            _repo = repo;
        }

        public override async Task<Result<GetLineResponse>> Handle(GetLineRequest request, CancellationToken cancellationToken)
        {
            var line = await _repo.GetLineByCodeAsync(request.LineCode).ConfigureAwait(false);
            if (string.IsNullOrEmpty(line.WorkOrder.Code))
            {
                return Fail($"La línea {request.LineCode} no tiene una orden de fabricación asociada.");
            }
            else if (string.Compare(line.ActivePart.Number, line.WorkOrder.PartNo, true) != 0)
            {
                return Fail($"La línea {request.LineCode} requiere cambio de modelo: {line.ActiveWorkOrderCode}: {line.ActivePart.Number} {line.ActivePart.Revision} >> {line.WorkOrder.Code}: {line.WorkOrder.PartNo} {line.WorkOrder.Revision}");
            }
            return OK(new GetLineResponse(line));
        }
    }
}
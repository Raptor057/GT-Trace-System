using GT.Trace.Changeover.App.Dtos;

namespace GT.Trace.Changeover.App.UseCases.GetLine
{
    public sealed record GetLineSuccessResponse(LineDto WorkOrder) : GetLineResponse;
}
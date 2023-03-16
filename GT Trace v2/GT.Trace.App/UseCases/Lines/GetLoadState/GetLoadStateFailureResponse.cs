using GT.Trace.Common;

namespace GT.Trace.App.UseCases.Lines.GetLoadState
{
    public record GetLoadStateFailureResponse(string Message) : GetLoadStateResponse;
}
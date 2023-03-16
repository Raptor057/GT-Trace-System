using GT.Trace.Common;

namespace GT.Trace.Packaging.App.UseCases.LoadPackState.Responses
{
    public abstract record FailureLoadPackStateResponse(string Message) : LoadPackStateResponse, IFailure;
}
using GT.Trace.Common;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadPackState.Responses
{
    public abstract record FailureLoadPackStateResponse(string Message) : LoadPackStateResponse, IFailure;
}
using GT.Trace.Common;

namespace GT.Trace.Packaging.App.UseCases.PackUnit.Responses
{
    public abstract record FailurePackUnitResponse(string Message) : PackUnitResponse, IFailure;
}
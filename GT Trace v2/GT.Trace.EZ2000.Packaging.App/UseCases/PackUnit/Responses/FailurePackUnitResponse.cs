using GT.Trace.Common;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit.Responses
{
    public abstract record FailurePackUnitResponse(string Message) : PackUnitResponse, IFailure;
}
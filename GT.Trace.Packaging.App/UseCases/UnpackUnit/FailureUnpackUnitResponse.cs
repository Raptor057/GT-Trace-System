using GT.Trace.Common;

namespace GT.Trace.Packaging.App.UseCases.UnpackUnit
{
    public record FailureUnpackUnitResponse(string Message) : UnpackUnitResponse, IFailure;
}
using GT.Trace.Common;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.UnpackUnit
{
    public record FailureUnpackUnitResponse(string Message) : UnpackUnitResponse, IFailure;
}
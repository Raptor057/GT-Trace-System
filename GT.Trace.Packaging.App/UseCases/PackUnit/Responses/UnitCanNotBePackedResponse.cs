using GT.Trace.Common;

namespace GT.Trace.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record UnitCanNotBePackedResponse(ErrorList Errors) : FailurePackUnitResponse(Errors.ToString());
}
using GT.Trace.Common;

namespace GT.Trace.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record InvalidLabelResponse(ErrorList Errors) : FailurePackUnitResponse(Errors.ToString());
}
using GT.Trace.Common;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record InvalidLabelResponse(ErrorList Errors) : FailurePackUnitResponse(Errors.ToString());
}
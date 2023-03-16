using GT.Trace.Common;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PrintPartialLabel.Responses
{
    public sealed record ValidationFailureResponse(ErrorList ValidationErrors) : FailurePrintPartialLabelResponse(ValidationErrors.ToString());
}
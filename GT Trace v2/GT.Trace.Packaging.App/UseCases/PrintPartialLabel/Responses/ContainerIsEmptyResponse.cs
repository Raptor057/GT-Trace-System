using GT.Trace.Common;

namespace GT.Trace.Packaging.App.UseCases.PrintPartialLabel.Responses
{
    public sealed record ValidationFailureResponse(ErrorList ValidationErrors) : FailurePrintPartialLabelResponse(ValidationErrors.ToString());
}
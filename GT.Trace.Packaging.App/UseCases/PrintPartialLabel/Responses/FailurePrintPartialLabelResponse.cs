using GT.Trace.Common;

namespace GT.Trace.Packaging.App.UseCases.PrintPartialLabel.Responses
{
    public abstract record FailurePrintPartialLabelResponse(string Message) : PrintPartialLabelResponse, IFailure;
}
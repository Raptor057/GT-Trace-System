using GT.Trace.Common;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PrintPartialLabel.Responses
{
    public abstract record FailurePrintPartialLabelResponse(string Message) : PrintPartialLabelResponse, IFailure;
}
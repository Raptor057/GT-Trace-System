using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.PrintPartialLabel.Responses;

namespace GT.Trace.Packaging.App.UseCases.PrintPartialLabel
{
    public sealed record PrintPartialLabelRequest(string Hostname) : IRequest<PrintPartialLabelResponse>;
}
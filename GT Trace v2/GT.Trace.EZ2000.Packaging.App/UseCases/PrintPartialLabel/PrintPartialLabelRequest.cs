using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.PrintPartialLabel.Responses;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PrintPartialLabel
{
    public sealed record PrintPartialLabelRequest(string Hostname) : IRequest<PrintPartialLabelResponse>;
}
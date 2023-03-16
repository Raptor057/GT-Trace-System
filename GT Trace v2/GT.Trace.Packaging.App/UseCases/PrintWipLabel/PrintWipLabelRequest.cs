using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.PrintWipLabel
{
    public sealed record PrintWipLabelRequest(string HostName) : IRequest<PrintWipLabelResponse>;
}
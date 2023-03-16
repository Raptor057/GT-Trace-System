using GT.Trace.Common.CleanArch;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PrintWipLabel
{
    public sealed record PrintWipLabelRequest(string HostName) : IRequest<PrintWipLabelResponse>;
}
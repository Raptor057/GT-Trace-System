using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.LoadPackState.Responses;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadPackState
{
    public record LoadPackStateRequest(string Hostname, int? PalletSize, int? ContainerSize, string? LineCode, string? PoNumber) : IRequest<LoadPackStateResponse>
    {
        public LoadPackStateRequest(string hostname)
            : this(hostname, null, null, null, null)
        { }
    }
}
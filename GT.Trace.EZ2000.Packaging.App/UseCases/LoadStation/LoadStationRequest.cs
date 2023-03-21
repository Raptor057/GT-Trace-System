using GT.Trace.Common.CleanArch;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadStation
{
    public record LoadStationRequest(string Hostname, int? PalletSize, int? ContainerSize, string? LineCode, string? PoNumber) : IResultRequest<LoadStationResponse>
    {
        public LoadStationRequest(string hostname)
            : this(hostname, null, null, null, null)
        { }
    }
}
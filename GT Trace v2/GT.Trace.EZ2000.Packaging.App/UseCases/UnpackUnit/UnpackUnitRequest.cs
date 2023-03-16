using GT.Trace.Common.CleanArch;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.UnpackUnit
{
    public sealed record UnpackUnitRequest(string LineCode, string ScanInput) : IRequest<UnpackUnitResponse>;
}
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.UnpackUnit
{
    public sealed record UnpackUnitRequest(string LineCode, string ScanInput) : IRequest<UnpackUnitResponse>;
}
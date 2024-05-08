using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.UnpackUnit
{
    public sealed record UnpackUnitRequest(string LineName, string ScanInput, string WorkOrderCode, string LineCode) : IRequest<UnpackUnitResponse>;
}
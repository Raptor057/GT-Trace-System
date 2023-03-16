using GT.Trace.Common.CleanArch;

namespace GT.Trace.EtiMovements.App.UseCases.GetEtiInfo
{
    public sealed record GetEtiInfoRequest(string? EtiInput) : IResultRequest<GetEtiInfoResponse>;
}
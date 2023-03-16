using GT.Trace.Common.CleanArch;

namespace GT.Trace.Etis.App.UseCases.GetEti
{
    public sealed record GetEtiRequest(long EtiID, string EtiNo) : IResultRequest<GetEtiResponse>;
}
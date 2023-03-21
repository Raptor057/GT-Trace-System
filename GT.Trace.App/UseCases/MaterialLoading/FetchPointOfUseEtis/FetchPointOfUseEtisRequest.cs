using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis
{
    public sealed record FetchPointOfUseEtisRequest(string PointOfUseCode) : IResultRequest<FetchPointOfUseEtisResponse>;
}
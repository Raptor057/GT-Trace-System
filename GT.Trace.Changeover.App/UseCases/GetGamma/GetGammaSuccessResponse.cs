using GT.Trace.Changeover.App.Dtos;

namespace GT.Trace.Changeover.App.UseCases.GetGamma
{
    public sealed record GetGammaSuccessResponse(IEnumerable<GammaItemDto> Gamma) : GetGammaResponse;
}
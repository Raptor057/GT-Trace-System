using GT.Trace.Packaging.App.Dtos;

namespace GT.Trace.Packaging.App.UseCases.PrintWipLabel
{
    public sealed record PrintWipLabelSuccessResponse(string LineCode, PalletDto Pallet) : PrintWipLabelResponse;
}
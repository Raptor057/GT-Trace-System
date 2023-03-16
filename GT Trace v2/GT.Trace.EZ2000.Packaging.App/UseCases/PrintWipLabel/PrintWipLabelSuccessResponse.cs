using GT.Trace.EZ2000.Packaging.App.Dtos;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PrintWipLabel
{
    public sealed record PrintWipLabelSuccessResponse(string LineCode, PalletDto Pallet) : PrintWipLabelResponse;
}
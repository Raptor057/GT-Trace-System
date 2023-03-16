using GT.Trace.EZ2000.Packaging.App.Dtos;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PrintPartialLabel.Responses
{
    public sealed record SuccessPrintPartialLabelResponse(string LineCode, PalletDto Pallet) : PrintPartialLabelResponse;
}
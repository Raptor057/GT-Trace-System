using GT.Trace.Packaging.App.Dtos;

namespace GT.Trace.Packaging.App.UseCases.PrintPartialLabel.Responses
{
    public sealed record SuccessPrintPartialLabelResponse(string LineCode, PalletDto Pallet) : PrintPartialLabelResponse;
}
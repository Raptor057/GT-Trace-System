namespace GT.Trace.Packaging.App.UseCases.LoadPackState.Dtos
{
    public record PalletDto(int Size, int Quantity, string PackagingImageBase64Data, string SampleImageBase64Data, ContainerDto Container, bool IsQcApproved);
}
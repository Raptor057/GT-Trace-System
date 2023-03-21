namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadPackState.Dtos
{
    public record PalletDto(int Size, int Quantity, string PackagingImageBase64Data, string SampleImageBase64Data, ContainerDto Container, bool IsQcApproved);
}
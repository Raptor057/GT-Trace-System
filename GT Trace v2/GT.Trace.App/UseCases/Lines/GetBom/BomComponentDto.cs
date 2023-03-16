namespace GT.Trace.App.UseCases.Lines.GetBom
{
    public sealed record BomComponentDto(string PartNo, string PartRev, string PointOfUseCode, int Capacity, string ComponentNo, string CompRev, int Quantity);
}
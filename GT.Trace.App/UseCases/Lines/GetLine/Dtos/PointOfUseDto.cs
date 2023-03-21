namespace GT.Trace.App.UseCases.Lines.GetLine.Dtos
{
    public sealed record PointOfUseDto(string Code, PartDto Component, decimal Cardinality, int Capacity, int Load, EtiDto? ActiveEti);
}
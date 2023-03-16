namespace GT.Trace.App.UseCases.Lines.GetBom
{
    public sealed record GetBomResponse(IEnumerable<BomComponentDto> Bom);
}
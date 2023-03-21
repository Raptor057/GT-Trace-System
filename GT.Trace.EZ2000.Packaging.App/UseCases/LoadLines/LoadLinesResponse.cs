namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadLines
{
    public sealed record LoadLinesResponse(IEnumerable<LineDto> Lines);
}

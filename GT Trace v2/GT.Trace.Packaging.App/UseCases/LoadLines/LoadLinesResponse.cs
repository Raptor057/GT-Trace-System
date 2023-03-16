namespace GT.Trace.Packaging.App.UseCases.LoadLines
{
    public sealed record LoadLinesResponse(IEnumerable<LineDto> Lines);
}
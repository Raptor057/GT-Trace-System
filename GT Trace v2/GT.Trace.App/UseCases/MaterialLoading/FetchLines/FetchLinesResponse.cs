namespace GT.Trace.App.UseCases.MaterialLoading.FetchLines
{
    public sealed record FetchLinesResponse(IEnumerable<LineDto> Lines);
}
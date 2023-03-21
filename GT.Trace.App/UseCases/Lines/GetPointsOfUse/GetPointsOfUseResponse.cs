namespace GT.Trace.App.UseCases.Lines.GetPointsOfUse
{
    public sealed record GetPointsOfUseResponse(IEnumerable<EnabledPointOfUseDto> EnabledPointsOfUse);
}
namespace GT.Trace.App.UseCases.Lines.GetHourlyProduction
{
    public sealed record GetHourlyProductionResponse(IEnumerable<string> Intervals, Dictionary<string, Dictionary<string, ProductionDto>> Production);
}
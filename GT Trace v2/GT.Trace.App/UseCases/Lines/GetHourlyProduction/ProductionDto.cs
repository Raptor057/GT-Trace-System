namespace GT.Trace.App.UseCases.Lines.GetHourlyProduction
{
    public sealed record ProductionDto(string Interval, int Requirement, string? PartNo, int? Quantity);
}
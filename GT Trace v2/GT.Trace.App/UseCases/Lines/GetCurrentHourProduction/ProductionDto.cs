namespace GT.Trace.App.UseCases.Lines.GetCurrentHourProduction
{
    public sealed record ProductionDto(string LineCode, string Interval, int ActualQuantity, int ExpectedQuantity, int Forecast, decimal ExpectedRate, decimal ActualRate, int Requirement);
}
namespace GT.Trace.Packaging.App.UseCases.LoadPackState.Dtos
{
    public sealed record HourlyProductionItemDto(string IntervalName, bool IsPastDue, bool IsCurrent, string PartNo, int TargetQuantity, int? Quantity);
}
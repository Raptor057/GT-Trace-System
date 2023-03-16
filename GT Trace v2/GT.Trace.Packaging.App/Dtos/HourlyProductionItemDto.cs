namespace GT.Trace.Packaging.App.Dtos
{
    public sealed record HourlyProductionItemDto(string IntervalName, bool IsPastDue, bool IsCurrent, string PartNo, int TargetQuantity, int? Quantity, decimal? Pph, int Headcount);
}
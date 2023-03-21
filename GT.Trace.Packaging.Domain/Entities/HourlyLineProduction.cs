namespace GT.Trace.Packaging.Domain.Entities
{
    public sealed record HourlyLineProcution(string IntervalName, bool IsPastDue, bool IsCurrent, string PartNo, int TargetQuantity, int? Quantity, DateTime? LastUpdateTime);
}
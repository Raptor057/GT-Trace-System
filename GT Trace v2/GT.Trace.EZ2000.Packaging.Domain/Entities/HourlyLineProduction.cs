namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    /// <summary>
    /// record de Producción de línea por hora
    /// </summary>
    /// <param name="IntervalName"></param>
    /// <param name="IsPastDue"></param>
    /// <param name="IsCurrent"></param>
    /// <param name="PartNo"></param>
    /// <param name="TargetQuantity"></param>
    /// <param name="Quantity"></param>
    /// <param name="LastUpdateTime"></param>
    public sealed record HourlyLineProcution(string IntervalName, bool IsPastDue, bool IsCurrent, string PartNo, int TargetQuantity, int? Quantity, DateTime? LastUpdateTime);
}

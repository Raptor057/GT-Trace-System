namespace GT.Trace.EZ2000.Packaging.App.Dtos
{
    //Objetos de transferencia de datos de tipo record;
    public sealed record HourlyProductionItemDto(string IntervalName, bool IsPastDue, bool IsCurrent, string PartNo, int TargetQuantity, int? Quantity, decimal? Pph, int Headcount);
}

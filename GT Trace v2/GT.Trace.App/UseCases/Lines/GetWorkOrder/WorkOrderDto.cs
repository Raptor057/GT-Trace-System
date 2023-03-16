namespace GT.Trace.App.UseCases.Lines.GetWorkOrder
{
    public sealed record WorkOrderDto(string Code, string PartNo, string Revision, int Size, int Quantity);
}

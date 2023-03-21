namespace GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID
{
    public sealed record GetNewSubAssemblyIDResponse(string LineCode, long EtiID, string ComponentNo, string Revision, string CompDescription, string WorkOrderCode, int Quantity, string TargetLineCode);
}
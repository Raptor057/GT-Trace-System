namespace GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID
{
    public interface IWorkOrderGateway
    {
        Task IncreaseWorkOrderQuantityAsync(int lineID, string workOrderCode, int amount);
    }
}
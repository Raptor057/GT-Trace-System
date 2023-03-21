namespace GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID
{
    public interface IGetNewSubAssemblyIDRepository
    {
        Task<long> ExecuteAsync(string lineCode, string componentNo, string revision, string workOrderCode, int quantity);
    }
}
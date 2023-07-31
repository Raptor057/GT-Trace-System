namespace GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID
{
    public interface IGetNewSubAssemblyIDRepository
    {
        //NOTE: ESTO PERTENECE A SUB ENSAMBLE
        Task<long> ExecuteAsync(string lineCode, string componentNo, string revision, string workOrderCode, int quantity);
    }
}
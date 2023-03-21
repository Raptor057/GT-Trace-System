namespace GT.Trace.Packaging.App.UseCases.LoadLines
{
    public interface ILinesDao
    {
        Task<IEnumerable<LineDto>> GetLinesAsync();
    }
}
namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadLines
{
    public interface ILinesDao
    {
        Task<IEnumerable<LineDto>> GetLinesAsync();
    }
}

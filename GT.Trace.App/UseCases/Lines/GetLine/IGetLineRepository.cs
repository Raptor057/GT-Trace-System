using GT.Trace.App.UseCases.Lines.GetLine.Dtos;

namespace GT.Trace.App.UseCases.Lines.GetLine
{
    public interface IGetLineRepository
    {
        Task<LineDto> GetLineByCodeAsync(string lineCode);
    }
}
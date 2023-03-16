namespace GT.Trace.App.UseCases.Lines.GetCurrentHourProduction
{
    public interface IGetCurrentHourProductionGateway
    {
        Task<ProductionDto?> GetProductionByLineAsync(string lineCode);
    }
}
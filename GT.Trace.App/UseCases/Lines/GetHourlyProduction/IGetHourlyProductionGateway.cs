namespace GT.Trace.App.UseCases.Lines.GetHourlyProduction
{
    public interface IGetHourlyProductionGateway
    {
        Task<IEnumerable<ProductionDto>> GetProductionByLineAsync(string lineCode, DateTime? workDayDate = null);
    }
}
namespace GT.Trace.App.UseCases.Lines.GetPointsOfUse
{
    public interface IGetPointsOfUseGateway
    {
        Task<IEnumerable<EnabledPointOfUseDto>> GetEnabledPointsOfUseAsync(string lineCode);
    }
}
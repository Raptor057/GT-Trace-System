namespace GT.Trace.Packaging.App.UseCases.SaveEzMotors
{
    public interface ISaveEzMotorsGateway
    {
        Task AddEZMotorsDataAsync(string model ,string serialNumber ,string lineCode);
        Task<bool> GetEzMotorsDataAsync(string model, string serialNumber, string lineCode);
    }
}

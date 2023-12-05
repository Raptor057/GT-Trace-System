namespace GT.Trace.Packaging.App.UseCases.SaveEzMotors
{
    public interface ISaveEzMotorsGateway
    {
        Task AddEZMotorsDataAsync(string model ,string serialNumber ,string lineCode,string pinionPartNum, string motorPartNum);
        Task<bool> GetEzMotorsDataAsync(string model, string serialNumber, string lineCode);
        Task<string> GetPignonByPartNoAsync(string partno);
        Task<string> GetMotorByPartNoAsync (string partno);
    }
}

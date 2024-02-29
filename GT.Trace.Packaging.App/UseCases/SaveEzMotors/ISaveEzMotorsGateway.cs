namespace GT.Trace.Packaging.App.UseCases.SaveEzMotors
{
    public interface ISaveEzMotorsGateway
    {
        Task AddEZMotorsDataAsync(string model, string serialNumber, string Volt, string RPM, DateTime DateTimeMotor, string Rev, string lineCode, string pinionPartNum, string motorPartNum);
        Task<bool> GetEzMotorsDataAsync(string model, string serialNumber, string lineCode, DateTime DateTimeMotor);
        Task<string> GetPignonByPartNoAsync(string partno, string lineCode);
        Task<string> GetMotorByPartNoAsync (string partno, string lineCode);
    }
}

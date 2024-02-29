namespace GT.Trace.Packaging.App.UseCases.JoinEZMotors
{
    public interface IJoinEZMotorsGateway
    {
        Task AddJoinEZMotorsAsync(long unitID, string Web1, string Current1, string Speed1, string Date1, string Time1, string Motor_Number1, string PN1, string AEM1,string Rev1, string Web2, string Current2, string Speed2, string Date2, string Time2, string Motor_Number2, string PN2, string AEM2, string Rev2);

        Task AddJoinEZMotorsAsync(long unitID, string Web, string Current, string Speed, string Date, string Time, string Motor_Number, string PN, string AEM, string Rev);

        Task DelJoinEZMotorsAsync(long unitID);
        //Task<int> EZRegisteredInformationUnitIDAsync(long unitID);
        //Task<int> EZRegisteredInformationComponentIDAsync(string componentID);
        Task<int> EZRegisteredInformationAsync(long unitID, string Date1, string Time1, string Motor_Number1, string Date2, string Time2, string Motor_Number2);
        Task<int> EZRegisteredInformationAsync(string Motor_Number, DateTime DateTimeMotor);
        Task<int> EZMotorDataRegisteredInformationAsync(string Motor_Number, DateTime DateTimeMotor);
    }
}

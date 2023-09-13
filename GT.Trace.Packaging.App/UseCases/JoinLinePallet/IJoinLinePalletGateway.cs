namespace GT.Trace.Packaging.App.UseCases.JoinLinePallet
{
    public interface IJoinLinePalletGateway
    {
        Task AddJoinPalletAsync(long UnitID, string PalletQR, string LineCode);

        Task<int> PalletRegisteredInformationAsync(long UnitID);
    }
}

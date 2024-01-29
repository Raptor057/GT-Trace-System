namespace GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot
{
    public interface ISaveSnapshotGateway
    {
        Task<string> SaveSnapshotAsync(string pointOfUseCode, string componentNo);
    }
}

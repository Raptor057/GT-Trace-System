namespace GT.Trace.BomSnapShot.App.Gateways
{
    public interface IGetSeqSnapshotIDGateways
    {
        Task<string> GetSeqSnapshotIDByLineCodeandPartNo(string lineCode, string partNo);
    }
}

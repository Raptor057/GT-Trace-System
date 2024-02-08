namespace GT.Trace.BomSnapShot.App.Gateways
{
    public interface IGetSeqSnapshotIDGateways
    {
        Task<long> GetSeqSnapshotIDByLineCodeandPartNo(string lineCode, string partNo);
    }
}

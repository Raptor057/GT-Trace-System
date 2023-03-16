namespace GT.Trace.Infra.Entities
{
    internal record BomComponent(
        short SeqNo, string PartNo, string PartRev, string PartFamily, string CompNo, string CompRev, string CompFamily,
        string CompDesc, string PointOfUse, string Capacity, int StdPackQty, int OperationNo, decimal ReqQty, string WorkshopCode, string CompRev2);
}
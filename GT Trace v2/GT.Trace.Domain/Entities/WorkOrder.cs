namespace GT.Trace.Domain.Entities
{
    public sealed class WorkOrder
    {
        public WorkOrder(long id, string code, Part part, string clientPartNo, string order, string lineOrder)
        {
            Id = id;
            Code = code;
            Part = part;
            ClientPartNo = clientPartNo;
            Order = order;
            LineOrder = lineOrder;
        }

        public long Id { get; }

        public string Code { get; }

        public Part Part { get; }

        public string ClientPartNo { get; }

        public string Order { get; }

        public string LineOrder { get; }
    }
}
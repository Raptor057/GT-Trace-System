using GT.Trace.Packaging.Domain.Enums;

namespace GT.Trace.Packaging.Domain.Entities
{
    public class WorkOrder
    {
        public WorkOrder(string code, int size, int quantity, Part part, string customerPartNo, PurchaseOrder po, Client client, MasterTypes masterType)
        {
            Code = code;
            Part = part;
            CustomerPartNo = customerPartNo;
            PO = po;
            Client = client;
            MasterType = masterType;
            Size = size;
            Quantity = quantity;
        }

        public string Code { get; }

        public int Size { get; }

        public int Quantity { get; }

        public Part Part { get; }

        public string CustomerPartNo { get; }

        public PurchaseOrder PO { get; }

        public Client Client { get; }

        public MasterTypes MasterType { get; }
    }
}
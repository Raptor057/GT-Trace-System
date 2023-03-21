namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    using GT.Trace.EZ2000.Packaging.Domain.Enums;
    public class WorkOrder
    {
        /// <summary>
        /// constructor de WorkOrder
        /// </summary>
        /// <param name="code"></param>
        /// <param name="size"></param>
        /// <param name="quantity"></param>
        /// <param name="part"></param>
        /// <param name="customerPartNo"></param>
        /// <param name="po"></param>
        /// <param name="client"></param>
        /// <param name="masterType"></param>
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

        /// <summary>
        /// Propiedades para el contrsuctor de WorkOrder
        /// </summary>
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

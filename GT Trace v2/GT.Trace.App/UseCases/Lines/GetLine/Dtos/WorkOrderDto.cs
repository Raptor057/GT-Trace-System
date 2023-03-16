namespace GT.Trace.App.UseCases.Lines.GetLine.Dtos
{
    public sealed class WorkOrderDto
    {
        public WorkOrderDto(string? code, string? client, int? size, int? quantity, int? stdPackSize, string? partNo, string? revision)
        {
            Code = code ?? "";
            Client = client ?? "";
            Size = size ?? 0;
            Quantity = quantity ?? 0;
            StdPackSize = stdPackSize ?? 0;
            PartNo = (partNo ?? "").Trim();
            Revision = (revision ?? "").Trim();
        }

        public string Code { get; }

        public string Client { get; }

        public int Size { get; }

        public int Quantity { get; }

        public int StdPackSize { get; }

        public string PartNo { get; }

        public string Revision { get; }
    }
}
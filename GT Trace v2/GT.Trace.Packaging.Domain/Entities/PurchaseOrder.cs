namespace GT.Trace.Packaging.Domain.Entities
{
    public record PurchaseOrder(string Number)
    {
        public bool IsValid => !string.IsNullOrWhiteSpace(Number);
    }
}
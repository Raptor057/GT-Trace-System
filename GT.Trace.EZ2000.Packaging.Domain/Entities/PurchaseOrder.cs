namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    /// <summary>
    /// Metodo boleano Orden de compra, recibe un valor string que es la orden de compra y la valida si no esta null o contiene espacios.
    /// </summary>
    /// <param name="Number"></param>
    public record PurchaseOrder(string Number)
    {
        public bool IsValid => !string.IsNullOrWhiteSpace(Number);
    }
}

namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    /// <summary>
    /// Record ID que recive el valor, contiene un metodo para crear ID nuevo que es igual a Datetime.now.ticks para el ID
    /// </summary>
    /// <param name="Value"></param>
    public record ID(long Value)
    {

        public static ID New() => new ID();
        public ID()
            : this(DateTime.Now.Ticks)
        { }
    }
}

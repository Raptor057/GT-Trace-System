namespace GT.Trace.Packaging.Domain.Entities
{
    public record ID(long Value)
    {
        public static ID New() => new ID();

        public ID()
            : this(DateTime.Now.Ticks)
        { }
    }
}
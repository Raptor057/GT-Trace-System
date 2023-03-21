namespace GT.Trace.Packaging.Domain.Entities
{
    public class MasterLabel
    {
        public MasterLabel(long id, IReadOnlyCollection<long> units)
        {
            ID = id;
            Units = units;
        }

        public MasterLabel(IReadOnlyCollection<long> units)
            : this(DateTime.Now.Ticks, units)
        { }

        public long ID { get; }

        public IReadOnlyCollection<long> Units { get; }
    }
}
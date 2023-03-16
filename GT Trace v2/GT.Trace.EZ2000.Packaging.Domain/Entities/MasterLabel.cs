namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    public class MasterLabel
    {
        /// <summary>
        /// Constructor MasterLabel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="units"></param>
        public MasterLabel(long id, IReadOnlyCollection<long> units)
        {
            ID = id;
            Units = units;
        }

        /// <summary>
        /// Establece el di de la etiqueta master como la fecha actual en formato Ticks y las unidades que contiene
        /// </summary>
        /// <param name="units"></param>
        public MasterLabel(IReadOnlyCollection<long> units)
            : this(DateTime.Now.Ticks, units)
        { }

        /// <summary>
        /// Propiedades de la clase usadas por el contrsuctor
        /// </summary>
        public long ID { get; }

        public IReadOnlyCollection<long> Units { get; }
    }
}

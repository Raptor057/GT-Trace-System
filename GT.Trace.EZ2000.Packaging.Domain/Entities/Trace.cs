namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    public class Trace
    {
        /// <summary>
        /// metodo que recibe herencia
        /// </summary>
        /// <param name="lineName"></param>
        /// <param name="utcScanMoment"></param>
        public Trace(string lineName, DateTime utcScanMoment)
            : this(ID.New(), lineName, utcScanMoment)
        { }

        /// <summary>
        /// Constructor de la clase con sus parametros
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lineName"></param>
        /// <param name="utcScanMoment"></param>
        public Trace(ID id, string lineName, DateTime utcScanMoment)
        {
            ID = id;
            LineName = lineName;
            UtcScanMoment = utcScanMoment;
        }
        /// <summary>
        /// Propiedades de la clase usadas por el constructor
        /// </summary>
        public ID ID { get; }

        public string LineName { get; }

        public DateTime UtcScanMoment { get; }
    }
}

namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    public class Unit
    {
        /// <summary>
        /// Constructor que recibe los siguientes parametros
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trace"></param>
        /// <param name="pickingReference"></param>
        /// <param name="functionalTestStatus"></param>
        /// <param name="masterIds"></param>
        /// <param name="packagingInfo"></param>
        /// <param name="currentProcessNo"></param>
        public Unit(long id, Trace? trace, long? pickingReference, bool? functionalTestStatus, IReadOnlyCollection<long> masterIds, PackagingInfo packagingInfo, string currentProcessNo)
        {
            ID = id;
            Trace = trace;
            PickingReference = pickingReference;
            FunctionalTestStatus = functionalTestStatus;
            MasterIds = masterIds;
            PackagingInfo = packagingInfo;
            CurrentProcessNo = currentProcessNo;
        }

        /// <summary>
        /// Propiedades de la clase usadas por el contrsuctor.
        /// </summary>
        public long ID { get; }

        public string CurrentProcessNo { get; }

        public long? PickingReference { get; }

        public bool? FunctionalTestStatus { get; }

        public Trace? Trace { get; private set; }

        public PackagingInfo PackagingInfo { get; }

        /// <summary>
        /// Averigur que hace esta propiedad.
        /// </summary>
        public bool IsTraced => Trace != null;
        /// <summary>
        /// Valida si la pieza paso por la prueba funcional.
        /// </summary>
        public bool IsTested => FunctionalTestStatus.HasValue;
        /// <summary>
        /// valida el estatus de la pureba funcional
        /// </summary>
        public bool FunctionalTestSucceeded => FunctionalTestStatus ?? false;

        /// <summary>
        /// Traza la pieza (ver mas a detalle que hace esto.)
        /// </summary>
        /// <param name="trace"></param>
        public void SetTrace(Trace trace)
        {
            Trace = trace;
        }
        /// <summary>
        /// ver a detalle que hace esto.
        /// </summary>
        public IReadOnlyCollection<long> MasterIds { get; }
    }
}

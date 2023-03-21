namespace GT.Trace.Packaging.Domain.Entities
{
    public class Unit
    {
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

        public long ID { get; }

        public string CurrentProcessNo { get; }

        public long? PickingReference { get; }

        public bool? FunctionalTestStatus { get; }

        public Trace? Trace { get; private set; }

        public PackagingInfo PackagingInfo { get; }

        public bool IsTraced => Trace != null;

        public bool IsTested => FunctionalTestStatus.HasValue;

        public bool FunctionalTestSucceeded => FunctionalTestStatus ?? false;

        public void SetTrace(Trace trace)
        {
            Trace = trace;
        }

        public IReadOnlyCollection<long> MasterIds { get; }
    }
}
namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    public class Approval
    {
        public Approval(bool isApproved, long? masterLabelReference)
        : this(ID.New(), isApproved, masterLabelReference, null, null)
        { }
        public Approval(ID id, bool isApproved, long? masterLabelReference, string? username, DateTime? date)
        {
            ID = id;
            IsApproved = isApproved;
            MasterLabelReference = masterLabelReference;
            Date = date;
            Username = username;
        }
        public ID ID { get; }

        public bool IsApproved { get; }

        public long? MasterLabelReference { get; private set; }

        public DateTime? Date { get; }

        public string? Username { get; }

        public void SetMasterLabelReference(long masterlabelReference)
        {
            MasterLabelReference = masterlabelReference;
        }
    }
}

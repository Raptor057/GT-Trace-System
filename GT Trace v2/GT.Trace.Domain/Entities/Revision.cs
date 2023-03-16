namespace GT.Trace.Domain.Entities
{
    public sealed class Revision
    {
        public static Revision New(string number) => new(number ?? "");

        private const string _prototypeRevisionInitialChars = "XY0";

        public Revision(string value)
        {
            OriginalValue = value.Trim().ToUpper();
            if (!string.IsNullOrWhiteSpace(OriginalValue))
            {
                IsPrototype = _prototypeRevisionInitialChars.Contains(OriginalValue[0]);
                Number = IsPrototype ? OriginalValue : OriginalValue[0].ToString();
            }
        }

        public string Number { get; } = "";

        public string OriginalValue { get; }

        public bool IsPrototype { get; } = false;

        public override string ToString() => Number;

        public override bool Equals(object? obj)
        {
            return (obj is Revision rev) && rev.Number == Number;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number);
        }
    }
}
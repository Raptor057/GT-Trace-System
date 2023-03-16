namespace GT.Trace.Packaging.Domain.Entities
{
    public record Part(string Number, Revision Revision, string? Description = null, string? ProductFamily = null)
    {
        public override int GetHashCode()
        {
            return HashCode.Combine(Number, Revision);
        }

        public override string ToString()
        {
            return $"{Number} Rev {Revision.Number}";
        }
    }
}
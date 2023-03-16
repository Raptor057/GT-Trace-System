namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    /// <summary>
    /// Record Part 
    /// </summary>
    /// <param name="Number"></param>
    /// <param name="Revision"></param>
    /// <param name="Description"></param>
    /// <param name="ProductFamily"></param>
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

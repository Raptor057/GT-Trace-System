namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    public record Label(long UnitID, Part Part, string ClientPartNo, string JulianDay);
}
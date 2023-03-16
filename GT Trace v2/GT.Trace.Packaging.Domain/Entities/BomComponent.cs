namespace GT.Trace.Packaging.Domain.Entities
{
    public record BomComponent(string Component, string Revision, string Description, string PointOfUse, string? EtiNo);
}
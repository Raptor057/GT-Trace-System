namespace GT.Trace.EZ2000.Packaging.Domain.Events
{
    public record PickingUpdatedEvent(long ID, bool IsActive, int Counter, int SequenceNo);
}

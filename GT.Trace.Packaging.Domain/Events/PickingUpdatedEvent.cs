namespace GT.Trace.Packaging.Domain.Events
{
    public record PickingUpdatedEvent(long ID, bool IsActive, int Counter, int SequenceNo);
}
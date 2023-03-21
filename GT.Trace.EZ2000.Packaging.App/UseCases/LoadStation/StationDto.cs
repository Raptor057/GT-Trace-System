namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadStation
{
    public record StationDto(
        string Hostname,
        string LineCode,
        string LineName,
        string ProcessName,
        bool IsLocked,
        bool CanTrace,
        bool CanSelectLine,
        bool CanAutoload,
        bool CanPickForTesting,
        bool FunctionalTestValidationIsRequired,
        bool CustomerPartNoValidationIsRequired,
        bool RevisionValidationIsRequired,
        string WorkOrderCode,
        string Part);
}
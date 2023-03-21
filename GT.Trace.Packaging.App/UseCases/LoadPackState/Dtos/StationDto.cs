namespace GT.Trace.Packaging.App.UseCases.LoadPackState.Dtos
{
    public record StationDto(string Hostname, string LineName, bool CanTrace, bool CanPick, bool CanCheckFunctionalTest, bool CanCheckPartNo, bool CanCheckRevision);
}
namespace GT.Trace.Packaging.App.UseCases.LoadPackState.Dtos
{
    public sealed record PickingDto(int Countdown, int Sequence, int TotalSamples, int Interval);
}
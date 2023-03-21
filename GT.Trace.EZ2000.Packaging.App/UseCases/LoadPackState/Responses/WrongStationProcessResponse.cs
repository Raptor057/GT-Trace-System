namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadPackState.Responses
{
    public sealed record WrongStationProcessResponse(string StationName, string ProcessName)
        : FailureLoadPackStateResponse($"El proceso de la estación {StationName} [{ProcessName}] no es empaque.");
}
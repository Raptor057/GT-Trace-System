namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadPackState.Responses
{
    public sealed record StationNotFoundResponse(string StationName)
        : FailureLoadPackStateResponse($"Estación con nombre \"{StationName}\" no encontrada.");
}
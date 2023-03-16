namespace GT.Trace.Packaging.App.UseCases.LoadPackState.Responses
{
    public sealed record StationNotFoundResponse(string StationName)
        : FailureLoadPackStateResponse($"Estación con nombre \"{StationName}\" no encontrada.");
}
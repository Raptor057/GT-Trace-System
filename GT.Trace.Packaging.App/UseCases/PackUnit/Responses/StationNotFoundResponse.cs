namespace GT.Trace.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record StationNotFoundResponse(string Hostname) : FailurePackUnitResponse($"Estación con nombre {Hostname} no encontrada.");
}
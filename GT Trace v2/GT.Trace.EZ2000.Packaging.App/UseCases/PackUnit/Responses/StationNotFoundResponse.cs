namespace GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record StationNotFoundResponse(string Hostname) : FailurePackUnitResponse($"Estación con nombre {Hostname} no encontrada.");
}
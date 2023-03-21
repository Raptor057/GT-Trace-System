namespace GT.Trace.EZ2000.Packaging.App.UseCases.PrintPartialLabel.Responses
{
    public sealed record StationNotFoundResponse(string Hostname) : FailurePrintPartialLabelResponse($"Estación con nombre {Hostname} no encontrada.");
}
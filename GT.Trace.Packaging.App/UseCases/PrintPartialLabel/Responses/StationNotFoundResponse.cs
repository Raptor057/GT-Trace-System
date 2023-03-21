namespace GT.Trace.Packaging.App.UseCases.PrintPartialLabel.Responses
{
    public sealed record StationNotFoundResponse(string Hostname) : FailurePrintPartialLabelResponse($"Estación con nombre {Hostname} no encontrada.");
}
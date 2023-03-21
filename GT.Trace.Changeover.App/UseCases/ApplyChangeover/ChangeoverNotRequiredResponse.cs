namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    /// <summary>
    /// Clase que representa una respuesta de que no se requiere un cambio de producción para una línea determinada.
    /// </summary>
    public sealed record ChangeoverNotRequiredResponse(string LineCode)
        : ApplyChangeoverFailureResponse($"La línea \"{LineCode}\" no requiere un cambio de modelo.");
}
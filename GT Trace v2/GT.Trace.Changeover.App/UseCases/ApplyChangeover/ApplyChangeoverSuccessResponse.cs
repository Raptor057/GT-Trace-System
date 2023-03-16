namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    /// <summary>
    /// Clase que representa una respuesta exitosa de una operación de cambio de producción.
    /// </summary>
    public sealed record ApplyChangeoverSuccessResponse(List<string> PrintExceptions) : ApplyChangeoverResponse;
}
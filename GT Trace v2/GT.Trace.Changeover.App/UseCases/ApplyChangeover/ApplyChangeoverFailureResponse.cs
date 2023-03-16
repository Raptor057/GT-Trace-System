namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    /// <summary>
    /// Clase abstracta que representa la respuesta de una operación de cambio de producción que ha fallado.
    /// </summary>
    public abstract record ApplyChangeoverFailureResponse(string Message) : ApplyChangeoverResponse;
}
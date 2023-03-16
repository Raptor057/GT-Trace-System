using GT.Trace.Common.CleanArch;

namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    /// <summary>
    /// Clase abstracta que representa la respuesta de una operación de cambio de producción.
    /// </summary>
    public abstract record ApplyChangeoverResponse() : IResponse;
}
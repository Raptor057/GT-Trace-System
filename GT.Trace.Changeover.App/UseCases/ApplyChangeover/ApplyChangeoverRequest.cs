using GT.Trace.Common.CleanArch;

namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    /// <summary>
    /// Clase que representa una solicitud para ejecutar una operación de cambio de producción.
    /// </summary>
    public sealed record ApplyChangeoverRequest(string LineCode) : IRequest<ApplyChangeoverResponse>;
}
using GT.Trace.Changeover.App.Dtos;

namespace GT.Trace.Changeover.App.Gateways
{
    /// <summary>
    /// Obtiene un objeto LineDto que corresponde a una línea de producción especificada por su código.
    /// </summary>
    public interface ILineGateway
    {

        /// <param name="lineCode">Código de la línea de producción.</param>
        /// <returns>Un objeto LineDto correspondiente a la línea de producción encontrada, o null si no se encontró ninguna.</returns>
        Task<LineDto?> GetLineAsync(string lineCode);

        /// <summary>
        /// Actualiza la orden de trabajo de una línea de producción específica.
        /// </summary>
        /// <param name="workOrder">Objeto WorkOrderDto que contiene los detalles de la orden de trabajo.</param>
        Task UpdateWorkOrderAsync(WorkOrderDto workOrder);
    }
}
using GT.Trace.Changeover.App.Dtos;

namespace GT.Trace.Changeover.App.Gateways
{
    /// <summary>
    /// Interfaz que define los métodos para acceder a los datos de las órdenes de trabajo.
    /// </summary>
    public interface IWorkOrderGateway
    {
        /// <summary>
        /// Obtiene la orden de trabajo activa de una línea de producción específica.
        /// </summary>
        /// <param name="lineID">ID de la línea de producción a buscar.</param>
        /// <returns>Un objeto WorkOrderDto que corresponde a la orden de trabajo activa de la línea de producción encontrada o null si no existe.</returns>
        Task<WorkOrderDto?> GetLineWorkOrderAsync(int lineID);
    }
}
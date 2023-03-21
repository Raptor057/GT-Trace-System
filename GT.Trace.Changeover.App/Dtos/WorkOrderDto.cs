namespace GT.Trace.Changeover.App.Dtos
{
    /// <summary>
    /// Representa una orden de trabajo en forma de objeto DTO.
    /// </summary>
    /// <summary>
    /// Crea una nueva instancia de la clase WorkOrderDto con los datos proporcionados.
    /// </summary>
    /// <param name="lineId">Identificador único de la línea de producción asociada a la orden de trabajo.</param>
    /// <param name="code">Código asignado a la orden de trabajo.</param>
    /// <param name="partNo">Número de parte asociado a la orden de trabajo.</param>
    /// <param name="revision">Revisión del número de parte asociado a la orden de trabajo.</param>
    public sealed record WorkOrderDto(int LineID, string Code, string PartNo, string Revision);
}
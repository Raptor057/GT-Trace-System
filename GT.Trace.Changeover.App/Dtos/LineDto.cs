namespace GT.Trace.Changeover.App.Dtos
{
    /// <summary>
    /// Representa una línea de producción en forma de objeto DTO.
    /// </summary>
    /// /// <summary>
    /// Crea una nueva instancia de la clase LineDto con los datos proporcionados.
    /// </summary>
    /// <param name="id">Identificador único de la línea de producción.</param>
    /// <param name="code">Código asignado a la línea de producción.</param>
    /// <param name="partNo">Número de parte asociado a la línea de producción.</param>
    /// <param name="revision">Revisión del número de parte asociado a la línea de producción.</param>
    /// <param name="workOrderCode">Código de orden de trabajo asociado a la línea de producción.</param>
    public sealed record LineDto(int ID, string Code, string PartNo, string Revision, string WorkOrderCode);
}
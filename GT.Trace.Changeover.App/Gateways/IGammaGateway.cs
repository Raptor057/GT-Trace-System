using GT.Trace.Changeover.App.Dtos;

namespace GT.Trace.Changeover.App.Gateways
{
    /// <summary>
    /// Interfaz que define los métodos para acceder a los datos de los componentes Gamma.
    /// </summary>
    public interface IGammaGateway
    {


        /// <summary>
        /// Obtiene una lista de objetos GammaItemDto que corresponden a los componentes Gamma asociados a una línea de producción,
        /// un número de parte y una revisión específicos.
        /// </summary>
        /// <param name="lineCode">Código de la línea de producción.</param>
        /// <param name="partNo">Número de parte de los componentes Gamma a buscar.</param>
        /// <param name="revision">Revisión del número de parte de los componentes Gamma a buscar.</param>
        /// <returns>Una lista de objetos GammaItemDto correspondientes a los componentes Gamma encontrados.</returns>
        Task<IEnumerable<GammaItemDto>> GetGammaAsync(string lineCode, string partNo, string revision);

        /// <summary>
        /// Obtiene una lista de objetos GammaItemDto que corresponden a los componentes Gamma salientes de un número de parte y una
        /// revisión de componente de entrada específicos.
        /// </summary>
        /// <param name="ogPartNo">Número de parte del componente de salida.</param>
        /// <param name="ogRevision">Revisión del número de parte del componente de salida.</param>
        /// <param name="icPartNo">Número de parte del componente de entrada.</param>
        /// <param name="icRevision">Revisión del número de parte del componente de entrada.</param>
        /// <returns>Una lista de objetos GammaItemDto correspondientes a los componentes Gamma encontrados.</returns>
        Task<IEnumerable<GammaItemDto>> GetOutgoingComponentsAsync(string ogPartNo, string ogRevision, string icPartNo, string icRevision);
    }
}
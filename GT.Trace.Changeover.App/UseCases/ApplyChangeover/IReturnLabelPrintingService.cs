namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    public interface IReturnLabelPrintingService
    {
        /// <summary>
        /// Ejecuta la operación de impresión de etiquetas de retorno para la línea y los ETIs especificados.
        /// </summary>
        /// <param name="lineCode">El código de línea para el cual se imprimirán las etiquetas de retorno.</param>
        /// <param name="etis">Un array de etiquetas ETI a imprimir.</param>
        /// <returns>Una tarea que resuelve a una lista de mensajes de error que pueden ocurrir durante la operación de impresión.</returns>
        Task<List<string>> ExecuteAsync(string lineCode, string[] etis);
    }
}
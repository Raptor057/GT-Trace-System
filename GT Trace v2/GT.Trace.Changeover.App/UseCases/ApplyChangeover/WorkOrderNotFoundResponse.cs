namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    /*Este código define un registro sellado llamado WorkOrderNotFoundResponse, que representa una respuesta de fallo para el caso de uso ApplyChangeover en la aplicación GT.Trace.Changeover. Esta respuesta se devuelve cuando no se encuentra una orden de fabricación asociada a la línea de producción especificada.
    El registro sellado tiene un constructor que toma un parámetro lineID de tipo entero. Este valor se utiliza para construir un mensaje de error personalizado que se pasa como argumento a la clase base ApplyChangeoverFailureResponse.
    En resumen, esta clase se utiliza para representar una respuesta de error cuando no se encuentra una orden de fabricación para una línea de producción determinada.*/
    public sealed record WorkOrderNotFoundResponse(int lineID)
        : ApplyChangeoverFailureResponse($"No se encontró la orden de fabricación para la línea #{lineID}.");
}
namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    /*Este es un registro sellado (sealed record) llamado LineNotFoundResponse. Este registro se utiliza como respuesta para indicar que una línea de producción específica no se encontró en el sistema. Este registro tiene un campo llamado LineCode, que es una cadena que representa el código de la línea que no se encontró.
    Además, el registro hereda de ApplyChangeoverFailureResponse y utiliza una cadena de formato para generar el mensaje de error que se muestra al usuario. En este caso, el mensaje de error indica que la línea no se encontró utilizando el código de línea proporcionado.
    */
    public sealed record LineNotFoundResponse(string LineCode)
        : ApplyChangeoverFailureResponse($"No se encontró la línea \"{LineCode}\".");
}
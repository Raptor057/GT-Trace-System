namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    public interface IProductionScheduleGateway
    {
        /*Este fragmento de código define una interfaz llamada IProductionScheduleGateway. Esta interfaz define un contrato que debe ser implementado por cualquier clase que quiera actuar como una puerta de enlace (Gateway) hacia la programación de producción en la aplicación.
        La interfaz define un único método llamado UpdateProductionSchedule, el cual acepta cuatro argumentos: lineCode, partNo, revision y workOrderCode. Estos argumentos representan los datos necesarios para actualizar la programación de producción de una línea de producción determinada.
        El método no devuelve nada, ya que su función es simplemente actualizar la programación de producción. Si se produce algún error durante la actualización, la implementación de la clase que implemente esta interfaz deberá manejarlo adecuadamente.
        Es importante destacar que esta interfaz es utilizada en la clase ApplyChangeoverHandler para actualizar la programación de producción después de que se haya aplicado un cambio de modelo en una línea de producción específica.*/
        Task UpdateProductionSchedule(string lineCode, string partNo, string revision, string workOrderCode);

        //Agregado para corregir el BUG que no se actualiza la tabla LineProductionSchedule al aplicar cambio de modelo en cualquier linea
        Task<bool> FindLineModelCapabilitiesAsync(string lineCode, string partNo);

        Task InsertModelCapabilitiesAsync(string lineCode, string partNo);
    }
}
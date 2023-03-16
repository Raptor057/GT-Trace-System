namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    public interface IPointOfUseGateway
    {
        /*
         * Este código define una interfaz llamada IPointOfUseGateway en el namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover. La interfaz especifica un método llamado GetLoadedComponentEtis que toma dos parámetros de tipo string, componentNo y pointOfUseCode, y devuelve una tarea que produce una secuencia de objetos EtiDto.
         * GetLoadedComponentEtis es un método que se utiliza para obtener los identificadores de las etiquetas ETI cargadas en un componente en un punto de uso específico en la línea de producción. Este método es utilizado por la clase ApplyChangeoverHandler para obtener las etiquetas ETI que se deben imprimir durante el cambio de modelo.
         * La definición de esta interfaz indica que el método GetLoadedComponentEtis debe ser implementado por cualquier clase que quiera actuar como un gateway para obtener los identificadores de las etiquetas ETI cargadas en un componente en un punto de uso específico.
         */
        Task<IEnumerable<EtiDto>> GetLoadedComponentEtis(string componentNo, string pointOfUseCode);
    }
}
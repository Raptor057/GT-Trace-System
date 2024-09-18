namespace GT.Trace.Packaging.Domain.Repositories
{
    using Domain.Entities;

    public interface IUnitRepository
    {
        Task<Unit?> GetUnitByIDAsync(long id);

        Task<ProTmsLineSerial?> GetLineAndSerialByIDAsync(long id);

        Task<long> AddUnitAsync(string lineCode, int position, string serialCode, DateTime creationTime);//NOTE: IMPORTANTE para motores lineas MW,MX

        Task AddMotorsDataAsync(string serialCode, string modelo, string volt, string rpm, DateTime dateTimeSerialCode, string rev);

        Task<long?> GetUnitIDBySerialCodeAsync(string serialCode);

        /// <summary>
        /// Agregado el 4/19/2024
        /// Obtiene la pagina Web directo de CEGID segun el nomero de parte y revision.
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="revision"></param>
        /// <returns>
        /// string con URL del manual para partes de servicio.
        /// </returns>
        Task <string?> GetWwwByCegidAsync(string partNo, string revision);
    }
}
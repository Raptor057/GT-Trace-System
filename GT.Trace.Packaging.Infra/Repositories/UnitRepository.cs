namespace GT.Trace.Packaging.Infra.Repositories
{
    using DataSources;
    using GT.Trace.Packaging.Domain.Entities;
    using GT.Trace.Packaging.Domain.Repositories;
    using GT.Trace.Packaging.Infra.DataSources.Entities;
    using Microsoft.Extensions.Logging;
    using System.Globalization;

    internal class UnitRepository : IUnitRepository
    {
        private readonly TrazaSqlDB _traza;
        private readonly AppsSqlDB _apps;
        private readonly GttSqlDB _gtt;
        private readonly CegidSqlDB _cegid;
        private readonly ILogger<UnitRepository> _logger;

        public UnitRepository(TrazaSqlDB traza, AppsSqlDB apps, GttSqlDB gtt, CegidSqlDB cegid, ILogger<UnitRepository> logger)
        {
            _traza = traza;
            _apps = apps;
            _gtt = gtt;
            _cegid=cegid;
            _logger=logger;
        }

        public async Task<Unit?> GetUnitByIDAsync(long id)
        {
            var unit = await _apps.TryGetUnitByIDAsync(id).ConfigureAwait(false);
            if (unit == null) return null;

            var trace = await _traza.TryGetUnitTraceAsync(id).ConfigureAwait(false);
            Trace? traceRef = null;
            PackagingInfo? packaging = null;
            if (trace != null)
            {
                traceRef = new Trace(ID.New(), trace.Linea, DateTime.ParseExact($"{trace.fecha_scan} {trace.hora_scan}", "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture));
            }

            var pickingRef = await _traza.TryGetUnitActivePickingReferenceAsync(id).ConfigureAwait(false);
            IEnumerable<dynamic> masters = await _traza.TryGetMasterIdsAssociatedToUnitAsync(id).ConfigureAwait(false);
            var packagingInfo = await _traza.TryGetPackagingInfoForUnitAsync(id).ConfigureAwait(false);
            if (packagingInfo != null)
            {
                packaging = new PackagingInfo(packagingInfo.linea, packagingInfo.fecha);
            }
            return new Unit(id, traceRef, pickingRef, unit.func_test_status, masters.Select(x => (long)x.Master_id).ToArray(), packaging, unit.curr_process_no);
        }

        public async Task<long> AddUnitAsync(string lineCode, int position, string serialCode, DateTime creationTime)
        {
            var line = await _apps.GetLineByCodeAsync(lineCode).ConfigureAwait(false);
            #pragma warning disable IDE0270 // Use coalesce expression
            if (line == null)
            {
                throw new InvalidOperationException($"Línea _{lineCode}_ no encontrada.");
            }
            #pragma warning restore IDE0270 // Use coalesce expression

            pro_production workOrder;
            var activeWorkOrder = await _gtt.GetActiveWorkOrderAsync(lineCode).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(activeWorkOrder))
            {
                workOrder = await _apps.GetWorkOrderByLineIDAsync(line.id).ConfigureAwait(false);
            }
            else
            {
                workOrder = await _apps.GetWorkOrderByCodeAsync(activeWorkOrder).ConfigureAwait(false);
            }

            //var workOrder = await _apps.GetWorkOrderByLineIDAsync(line.id, line.codew).ConfigureAwait(false);
            if (workOrder == null)
            {
                throw new InvalidOperationException($"Orden de fabricación \"{line.codew}\" asociada a la línea _{line.letter} (#{line.id})_ no encontrada.");
            }

            return await _apps.AddUnitAsync(position, creationTime, workOrder.part_number, workOrder.rev, workOrder.ratio, line.Tipo, workOrder.id, lineCode, serialCode).ConfigureAwait(false);
        }

        public async Task<long?> GetUnitIDBySerialCodeAsync(string serialCode) =>
            await _apps.GetUnitIDBySerialCodeAsync(serialCode).ConfigureAwait(false);

        public async Task AddMotorsDataAsync(string serialCode, string modelo, string volt, string rpm, DateTime dateTimeSerialCode, string rev)
        {
            await _gtt.AddMotorsData(serialCode,modelo,volt,rpm,dateTimeSerialCode,rev).ConfigureAwait(false);
        }

        public async Task<ProTmsLineSerial?> GetLineAndSerialByIDAsync(long id)
        {
           return await _apps.GetLineAndSerialByID(id).ConfigureAwait(false);
        }

        public async Task<string?> GetWwwByCegidAsync(string partNo, string revision)
            => await _cegid.GetWwwByCegid(partNo, revision).ConfigureAwait(false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitID"></param>
        /// <param name="lineCode"></param>
        /// <param name="PartNo"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> ValidateFunctionalTestAsync(long UnitID, string lineCode, string PartNo)
        {
            var ValidationDataForFunctionalTesting = await _gtt.ValidationDataForFunctionalTestingByModelAndLine(lineCode,PartNo).ConfigureAwait(false);

            if(ValidationDataForFunctionalTesting == null)
            {
                _logger.LogInformation($"No se encontraron datos de validacion de pureba funcional en la tabla [LineModelFunctionalTest] en la base de datos GTT, para el modelo {PartNo} en la linea {lineCode}, esto significa que para esta linea y modelo no es necesario validar la prueba funcional");

                return true;
            }
            
            var ResultOfFunctionalTestByUnitID = await _apps.GetFunctionalTestResultOfLastPrintedLabelByLineAndModelAndUnitID(UnitID, lineCode, PartNo).ConfigureAwait(false);

            if (ResultOfFunctionalTestByUnitID == null)
            {
                _logger.LogError($"No se encontraron datos de pureba funcional en la tabla [pro_tms] de la TM {UnitID} en la base de datos APPS");

                throw new InvalidOperationException($"No se encontraron datos de pureba funcional en la tabla [pro_tms] de la TM {UnitID} en la base de datos APPS");
                //return false;
            }

            if(ResultOfFunctionalTestByUnitID.functional_test_final_result == null)
            {
                _logger.LogError($"No hay datos sobre el resultado de la prueba funcional de la TM {UnitID}");

                throw new InvalidOperationException($"No hay datos sobre el resultado de la prueba funcional de la TM {UnitID}");
            }

            if(!ResultOfFunctionalTestByUnitID.functional_test_final_result.Value && ResultOfFunctionalTestByUnitID.functional_test_count.Value > 0 && ResultOfFunctionalTestByUnitID.functional_test_count.Value <= 5)
            {
                _logger.LogError($"La prueba funcional no fue exitosa, vuelve a escanear la TM e intenta realizar la prueba funcional nuevamente, intento {5- ResultOfFunctionalTestByUnitID.functional_test_count}/5");

                throw new InvalidOperationException($"La prueba funcional no fue exitosa, vuelve a escanear la TM e intenta realizar la prueba funcional nuevamente, intento {5 - ResultOfFunctionalTestByUnitID.functional_test_count}/5");
            }

            if (ResultOfFunctionalTestByUnitID.functional_test_final_result.Value || !ResultOfFunctionalTestByUnitID.functional_test_final_result.Value && ResultOfFunctionalTestByUnitID.functional_test_count.Value > 5)
            {
                _logger.LogError($"La prueba funcional no fue exitosa para la TM {UnitID}, y cuenta con un total de {ResultOfFunctionalTestByUnitID.functional_test_count.Value} intentos fallidos superando el limite establecido de maximo 5 pruebas, pasa a la siguiente TM, o comunicate con calidad.");

                throw new InvalidOperationException($"\"La prueba funcional no fue exitosa para la TM {{UnitID}}, y cuenta con un total de {{ResultOfFunctionalTestByUnitID.functional_test_count.Value}} intentos fallidos superando el limite establecido de maximo 5 pruebas, pasa a la siguiente TM, o comunicate con calidad.");
            }

            if (ResultOfFunctionalTestByUnitID.functional_test_final_result.Value) 
            {
                _logger.LogInformation($"TM {UnitID} con prueba funcional exitosa");
                return true;
            }

            _logger.LogError($"Por algun motivo algo salio mal con la TM {UnitID}, revisa la informacion de la TM en la tabla [APPS].[dbo].[pro_tms] o la validacion de prueba funcional por linea y modelo en la tabla [gtt].[dbo].[LineModelFunctionalTest]");

            throw new InvalidOperationException($"Por algun motivo algo salio mal con la TM {UnitID}, revisa la informacion de la TM en la tabla [APPS].[dbo].[pro_tms] o la validacion de prueba funcional por linea y modelo en la tabla [gtt].[dbo].[LineModelFunctionalTest]");
        }
    }
}
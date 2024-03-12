namespace GT.Trace.Packaging.Infra.Repositories
{
    using DataSources;
    using GT.Trace.Common.Infra.DataSources.SqlDB;
    using GT.Trace.Packaging.Domain.Entities;
    using GT.Trace.Packaging.Domain.Repositories;
    using GT.Trace.Packaging.Infra.DataSources.Entities;
    using System.Globalization;

    internal class UnitRepository : IUnitRepository
    {
        private readonly TrazaSqlDB _traza;
        private readonly AppsSqlDB _apps;
        private readonly GttSqlDB _gtt;

        public UnitRepository(TrazaSqlDB traza, AppsSqlDB apps, GttSqlDB gtt)
        {
            _traza = traza;
            _apps = apps;
            _gtt = gtt;
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
    }
}
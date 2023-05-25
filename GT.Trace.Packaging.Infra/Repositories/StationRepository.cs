namespace GT.Trace.Packaging.Infra.Repositories
{
    using DataSources;
    using GT.Trace.Packaging.Domain.Entities;
    using GT.Trace.Packaging.Domain.Events;
    using GT.Trace.Packaging.Domain.Repositories;

    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    internal class StationRepository : IStationRepository
    {
        public const string PackagingProcessNo = "999";

        private static string GetLineCode(bool canChangeLine, string? selectedLineCode, string stationLineName)
        {
            if (!canChangeLine && !string.IsNullOrWhiteSpace(selectedLineCode))
            {
                throw new Exception("No se puede cambiar de línea.");
            }

            var lineParts = stationLineName.Split(' ');
            if (!canChangeLine && (lineParts.Length < 2 || string.IsNullOrWhiteSpace(lineParts[1])))
            {
                throw new Exception("La estación no tiene asociada una línea.");
            }

            return canChangeLine && !string.IsNullOrWhiteSpace(selectedLineCode)
                ? selectedLineCode
                : lineParts[1];
        }

        private readonly TrazaSqlDB _traza;
        private readonly AppsSqlDB _apps;
        private readonly CegidSqlDB _cegid;
        private readonly GttSqlDB _gtt;

        //private readonly ILogger<StationRepository> _logger;

        public StationRepository(TrazaSqlDB traza, CegidSqlDB cegid, AppsSqlDB apps, GttSqlDB gtt/*, ILogger<StationRepository> logger*/)
        {
            _traza = traza;
            _apps = apps;
            _cegid = cegid;
            //_logger = logger;
            _gtt = gtt;
        }

        private Station? _station;

        public async Task<Station> GetStationByHostnameAsync(string hostname, string? lineCode, int? palletSize, int? containerSize, string? poNumber)
        {
            var pcmx = await _traza.TryGetStationByHostnameAsync(hostname).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"Estación \"{hostname}\" no encontrada.");


            var selectedLineCode = GetLineCode(pcmx.Can_Chg_Line ?? false, lineCode, pcmx.LINE);

            var lineHeadcount = await _gtt.GetLineHeadcountAsync(selectedLineCode).ConfigureAwait(false);

            var prod_unit = await _apps.GetLineByCodeAsync(selectedLineCode).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"Línea \"{selectedLineCode}\" no encontrada.");

            if (string.IsNullOrWhiteSpace(prod_unit.modelo))
                throw new InvalidOperationException($"No existe modelo asociado a la línea \"{prod_unit.letter}\", se requiere cambio de modelo.");

            DataSources.Entities.pro_production? production = null;
            var activeWorkOrder = await _gtt.GetActiveWorkOrderAsync(selectedLineCode).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(activeWorkOrder))
            {
                production = await _apps.GetWorkOrderByLineIDAsync(prod_unit.id).ConfigureAwait(false)
                    ?? throw new InvalidOperationException($"No hay orden de fabricación activa asociada a la línea #{prod_unit.id} ({prod_unit.comments}).");
            }
            else
            {
                production = await _apps.GetWorkOrderByCodeAsync(activeWorkOrder).ConfigureAwait(false)
                    ?? throw new InvalidOperationException($"No hay orden de fabricación activa asociada a la línea #{prod_unit.id} ({prod_unit.comments}).");
                prod_unit.modelo = production.part_number.Trim();
                prod_unit.active_revision = production.rev.Trim();
                prod_unit.codew = production.codew.Trim();
            }

            var revision = Revision.New(production.rev);

            var refext = await _cegid.GetRefExtAsync(production.part_number.Trim(), revision.Number, production.client_code ?? 0).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"No hay información de empaque para {production.part_number.Trim()} Rev {revision.Number}, cliente: {production.client_code}");

            var client = await _cegid.GetClientAsync(production.client_code ?? 0).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"Cliente #{production.client_code} no encontrado.");

            var uarticle = await _cegid.GetUarticleAsync(production.part_number.Trim(), revision.Number).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"No se encontró información de contenedores para {production.part_number.Trim()} Rev {revision.Number}.");

            var article = await _cegid.GetArticleAsync(production.part_number.Trim(), revision.Number).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"No se encontró información de parte para {production.part_number.Trim()} Rev {revision.Number}.");

            var scannedUnits = await _traza.GetScannedUnitsByLineAsync(prod_unit.comments, prod_unit.modelo).ConfigureAwait(false);

            var packedUnits = await _traza.GetPackedUnitsByWorkOrderAsync(production.codew).ConfigureAwait(false);

            var picking_config = await _traza.GetPickingConfigAsync(article.Family.Trim()).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"No se encontró configuración de picking para la sub familia \"{article.Family.Trim()}\",");

            var qc_params = await _traza.GetContainerApprovalParamsAsync(picking_config.tipo, refext.PackType.Trim()).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"No se encontraron parametros de aprobación de calidad para tipo \"{picking_config.tipo}\" con empaque \"{refext.PackType.Trim()}\".");


            #region Modulo agregado por 5 why, pockayoke agregado por problemas en ****** produccion ***** 2/5/2023
            var stationIsBlocked = await _traza.TryGetStationIsBlockedAsync(hostname).ConfigureAwait(false);
            var messagefromassembly = await _gtt.GetMessageFromAssembly(selectedLineCode).ConfigureAwait(false);
            if (stationIsBlocked.is_blocked == true)
                throw new InvalidCastException($"No se puede continuar. La estación {hostname} de la linea {selectedLineCode} está bloqueada debido a: [{messagefromassembly}]." +
                    $"Ingrese la contraseña de supervisor / inspector de calidad en la estación de ensamble para continuar.");
            #endregion

            var qc_approval = await _traza.TryGetApprovalByWorkOrderAsync(production.codew.Trim()).ConfigureAwait(false);

            var intervalProduction = Enumerable.Empty<DataSources.Entities.ProductionTimeInterval>();

            //En esta linea se valida el proceso
            var validSourceProcessesForPackagingProcess = (await _gtt.GetLineRoutingByLineCode(prod_unit.letter.Trim().ToUpper()).ConfigureAwait(false))
                .Where(item => item.ProcessNo == PackagingProcessNo)
                .Select(item => item.PrevProcessNo)
                .ToArray();

            var picking_counter = await _traza.GetPickingCounterAsync((int)picking_config.Id, production.part_number.Trim(), revision.Number).ConfigureAwait(false);
            if (picking_counter == null)
            {
                picking_counter = new();
                picking_counter.Id = await _traza.CreatePickingCounterAsync((int)picking_config.Id, production.part_number.Trim(), revision.Number).ConfigureAwait(false);
                picking_counter.REV = production.part_number.Trim();
                picking_counter.is_active = false;
                picking_counter.id_ssFam = (int)picking_config.Id;
                picking_counter.NP = production.part_number.Trim();
            }
            var bom = await _traza.GetBomAsync(production.part_number, prod_unit.letter).ConfigureAwait(false);
            var set = await _gtt.GetActiveSetByLineAsync(prod_unit.letter).ConfigureAwait(false);

            if ((palletSize ?? 0) == 0) palletSize = uarticle.PalletSize;
            if ((containerSize ?? 0) == 0) containerSize = uarticle.ContainerSize;
            var containerPlaceholders = Enumerable.Range(0, scannedUnits.Count() / (containerSize ?? 0) + 1);

            var purchaseOrder = new PurchaseOrder(poNumber ?? refext.PO.Trim());
            var workOrder = new WorkOrder(
                production.codew.Trim(),
                production.target_qty ?? 0,
                production.current_qty ?? 0,
                new Part(
                    production.part_number.Trim(),
                    Revision.New(production.rev.Trim()),
                    production.part_desc.Trim(),
                    article.Family.Trim()),
                production.ref_ext.Trim(),
                purchaseOrder,
                new Client(client.Code, client.Name, client.Description),
                uarticle.MasterTypeCode == "1" ? Domain.Enums.MasterTypes.Master : Domain.Enums.MasterTypes.ATEQ);

            Approval? approval = null;

            if (qc_approval != null)
            {
                DateTime? approvalDate = null;
                if (!string.IsNullOrWhiteSpace(qc_approval.fecha_app))
                {
                    if (DateTime.TryParseExact(qc_approval.fecha_app, "dd-MMM-yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                    {
                        approvalDate = parsedDate;
                    }
                }
                approval = new Approval(new ID(qc_approval.Id), qc_approval.is_approved ?? false, qc_approval.ID_MASTER, qc_approval.user_app, approvalDate);
            }

            var pallet = Pallet.Create(palletSize!.Value, containerSize!.Value, refext.PackType == "B" ? Domain.Enums.ContainerType.Box : Domain.Enums.ContainerType.CollapsibleBulkContainer, approval,
                containerPlaceholders.Select(
                    i => Container.Create(
                        i + 1,
                        containerSize!.Value,
                        refext.PackType == "B" ? Domain.Enums.ContainerType.Box : Domain.Enums.ContainerType.CollapsibleBulkContainer,
                        scannedUnits
                            .Skip(i * containerSize!.Value)
                            .Take(containerSize!.Value)
                            .Select(t => long.Parse(t.telesis))
                            .ToList()
                    )
                ).ToList());

            var picking = new Picking(new ID(picking_counter.Id), picking_config.Freq ?? int.MaxValue, picking_counter.Counter ?? 0, picking_counter.tot_test ?? 0, picking_config.total_samples);
            _station = new Station(
                pcmx.PROCESSNAME,
                pcmx.is_blocked ?? false,
                pcmx.Can_Val_Rev ?? false,
                pcmx.Can_Val_custpn ?? false,
                pcmx.Can_Val_Benchtest ?? false,
                false,
                pcmx.Can_Save_traza ?? false,
                pcmx.Can_pick_qctest ?? false,
                pcmx.Can_autoload ?? false,
                pcmx.Can_Chg_Line ?? false,
                new Line(
                    prod_unit.id,
                    prod_unit.letter.Trim().ToUpper(),
                    prod_unit.comments.Trim().ToUpper(),
                    lineHeadcount?.Headcount ?? 0,
                    pallet, workOrder, picking,
                new Bom(
                    bom.Select(item =>
                        new BomComponent(
                            item.CompNo,
                            item.CompRev,
                            item.CompDesc,
                            item.PointOfUse,
                            set.FirstOrDefault(s =>
                                s.ComponentNo.Trim() == item.CompNo &&
                                //new Domain.Revision(s.rev_cc).Equals(new Domain.Revision(item.CompRev)) &&
                                s.PointOfUseCode.Trim() == item.PointOfUse)?.EtiNo)
                ).ToList()),
                new Part(prod_unit.modelo.Trim(), Revision.New(prod_unit.active_revision), "", ""),
                qc_params.PARAM_INI ?? int.MaxValue,
                qc_params.PARAM_FIN ?? int.MaxValue,
                intervalProduction.Select(prod => new HourlyLineProcution(prod.Name, prod.IsPastDue, prod.IsCurrent, prod.PartNo, prod.EffectiveHourlyRequirement, prod.Quantity, prod.LastUpdateTime)),
                validSourceProcessesForPackagingProcess));

            return _station;
        }

        public async Task SaveAsync(Station station)
        {
            //using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            foreach (var e in station.GetEvents())
            {
                if (e is PalletCompletedEvent palletCompleted)
                {
                    await OnPalletCompletedAsync(palletCompleted).ConfigureAwait(false);
                }
                else if (e is PickingUpdatedEvent pickingUpdated)
                {
                    await OnPickingUpdatedAsync(pickingUpdated).ConfigureAwait(false);
                }
                else if (e is UnitPackedEvent unitPacked)
                {
                    await OnUnitPackedAsync(unitPacked).ConfigureAwait(false);
                }
                else if (e is UnitPickedEvent unitPicked)
                {
                    await OnUnitPickedAsync(unitPicked).ConfigureAwait(false);
                }
                else if (e is UnitTracedEvent unitTraced)
                {
                    await OnUnitTracedAsync(unitTraced).ConfigureAwait(false);
                }
                else if (e is ContainerApprovalCreatedEvent containerApprovalCreated)
                {
                    await OnContainerApprovalCreatedAsync(containerApprovalCreated).ConfigureAwait(false);
                }
            }
            //scope.Complete();
        }

        private async Task OnPalletCompletedAsync(PalletCompletedEvent e)
        {
            var masterID = await _traza.CreateMasterAsync(e.ClientName, e.PartNo, e.Revision, e.CustomerPartNo, e.PartDescription, e.ClientDescription, e.PO, e.LotNo, e.LineName, e.IsClosed, e.Quantity, e.MasterType.ToUpper(),
                e.WasPartial, e.IsPartial, e.JulianDay, e.ProductFamily, e.WorkOrderCode, e.ApprovalID, e.ApprovalUser, e.ApprovalDate).ConfigureAwait(false);
            await _traza.CopyPackedUnitsToMasterAsync(masterID, e.PartNo, e.LineName).ConfigureAwait(false);
            await _traza.DeleteTemporaryPalletAsync(e.LineName, e.ClientName, e.PartNo).ConfigureAwait(false);
            await _traza.UpdateApprovalAsync(e.ApprovalID, masterID, e.Quantity, e.JulianDay).ConfigureAwait(false);
        }

        private async Task OnPickingUpdatedAsync(PickingUpdatedEvent e)
        {
            await _traza.UpdatePickingCounterAsync(e.ID, e.Counter, e.IsActive, e.SequenceNo).ConfigureAwait(false);
        }

        private async Task OnUnitPackedAsync(UnitPackedEvent e)
        {
            await _traza.AddPackedUnitAsync(e.LineName, e.ClientName, e.UnitID, e.PartNo, e.JulianDay, e.IsPartial, e.MasterID, e.ApprovalID, e.WorkOrderCode, e.LineID).ConfigureAwait(false);
            await _gtt.RecordProductionAsync(e.LineCode, e.PartNo, e.Revision, e.WorkOrderCode, 1).ConfigureAwait(false);
            await _gtt.RecordPackagingProcessHistoryAsync(e.UnitID, e.LineCode).ConfigureAwait(false);
        }

        private async Task OnUnitPickedAsync(UnitPickedEvent e)
        {
            await _traza.AddPickingUnitAsync(e.UnitID, e.LineName, e.PartNo, e.SequenceNo, e.WorkOrderCode).ConfigureAwait(false);
        }

        private async Task OnUnitTracedAsync(UnitTracedEvent e)
        {
            await _traza.AddTracedUnitAsync(e.UnitID, e.LineName, e.Operation, e.ClientName, e.PartNo, e.WorkOrderCode).ConfigureAwait(false);
        }

        private async Task OnContainerApprovalCreatedAsync(ContainerApprovalCreatedEvent e)
        {
            await _traza.CreateApprovalAsync(e.LineName, e.PartNo, e.WorkOrderCode, e.PalletSize, e.CustomerPartNo, e.Revision, e.ContainerSize).ConfigureAwait(false);
        }

        public async Task<long?> GetLatestMasterLabelFolioByLineAsync(string lineName) =>
            await _traza.GetLastMasterFolioByLineAsync(lineName).ConfigureAwait(false);

        public async Task <string?> GetOrigenByCegid(string partNum, string partRev) =>
            await _traza.GetOrigenByCegid(partNum, partRev).ConfigureAwait(false);
    }
}
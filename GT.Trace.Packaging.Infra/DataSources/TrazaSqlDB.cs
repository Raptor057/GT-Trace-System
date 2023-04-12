using GT.Trace.Packaging.Infra.DataSources.Entities;

namespace GT.Trace.Packaging.Infra.DataSources
{
    internal class TrazaSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public TrazaSqlDB(ConfigurationSqlDbConnection<TrazaSqlDB> con)
        {
            _con = con;
        }

        public async Task<int> GetCurrentContainerQuantity(string lineCode, string partNo) =>
            await _con.ExecuteScalarAsync<int>("", new { lineCode, partNo }).ConfigureAwait(false);

        public async Task<IEnumerable<dynamic>> TryGetMasterIdsAssociatedToUnitAsync(long unitID) =>
            await _con.QueryAsync<dynamic>("SELECT x.* FROM dbo.Master_lbl_TMid x\r\nJOIN Master_labels_WB m ON m.id = x.Master_id and m.is_active = 1\r\nWHERE x.TM_id = @unitID;", new { unitID = unitID.ToString() }).ConfigureAwait(false);

        public async Task<dynamic> TryGetUnitTraceAsync(long unitID) =>
                await _con.QueryFirstAsync<dynamic>("SELECT * FROM dbo.Trazab_WB WHERE Telesis_no=@id;", new { id = unitID.ToString() });

        public async Task<long?> TryGetUnitActivePickingReferenceAsync(long unitID) =>
            await _con.ExecuteScalarAsync<long?>("SELECT TOP 1 Id FROM tbl_picking_WB WHERE Telesis_no=@unitID AND is_released = 0;", new { unitID = unitID.ToString() }).ConfigureAwait(false);

        public async Task<dynamic> TryGetPackagingInfoForUnitAsync(long unitID) =>
                    await _con.QueryFirstAsync<dynamic>("SELECT TOP 1 fecha, linea FROM dbo.Temp_pack_WB WHERE telesis = @id ORDER BY fecha DESC;", new { id = unitID.ToString() }).ConfigureAwait(false);

        public async Task<pcmx> TryGetStationByHostnameAsync(string hostname) =>
            await _con.QuerySingleAsync<pcmx>("SELECT * FROM dbo.pcmx WHERE PCNAME = @hostname;", new { hostname }).ConfigureAwait(false);

        #region Bloqueo de Linea y Validacion de Bloqueo
        //Validacion agregada para saber si empaque esta bloqueado o no 2/5/2023.
        public async Task<pcmx> TryGetStationIsBlockedAsync(string hostname) =>
            await _con.QuerySingleAsync<pcmx>("SELECT is_blocked FROM dbo.pcmx WHERE PCNAME = @hostname;", new { hostname }).ConfigureAwait(false);

        //Metodo agregado para bloquear empaque 2/8/2023
        public async Task<pcmx> SetStationBlocked(string is_blocked, string lineName) =>
            await _con.ExecuteScalarAsync<pcmx>("UPDATE pcmx SET is_blocked = @is_blocked WHERE LINE LIKE('%'+@lineName);", new { is_blocked, lineName }).ConfigureAwait(false);
        //Esto solo fue una prueba para validar la execucion de la linea
        //public async Task<pcmx> SetStationBlocked(string hostname) =>
        //    await _con.ExecuteScalarAsync<pcmx>("UPDATE pcmx SET is_blocked = 1 WHERE PCNAME = @hostname;", new { hostname}).ConfigureAwait(false);


        #endregion
        public async Task<IEnumerable<Temp_pack_WB>> GetScannedUnitsByLineAsync(string lineName, string partNo) =>
                    await _con.QueryAsync<Temp_pack_WB>("SELECT * FROM dbo.Temp_pack_WB WHERE num_p = @partNo AND linea = @lineName;", new { partNo, lineName }).ConfigureAwait(false);

        public async Task<IEnumerable<Master_lbl_TMid>> GetPackedUnitsByWorkOrderAsync(string workOrderCode) =>
                    await _con.QueryAsync<Master_lbl_TMid>(@"SELECT x.*
FROM dbo.Master_labels_WB m
JOIN dbo.Master_lbl_TMid x
    ON x.Master_id = m.Id
WHERE m.codew = @workOrderCode;", new { workOrderCode }).ConfigureAwait(false);

        public async Task<TZ_tblWBTest_Freq> GetPickingConfigAsync(string family) =>
            await _con.QuerySingleAsync<TZ_tblWBTest_Freq>("SELECT * FROM TZ_TBLWBTEST_FREQ WHERE cegidSF=@family;", new { family }).ConfigureAwait(false);

        public async Task<TZ_tblWBTest_counters> GetPickingCounterAsync(int periodID, string partNo, string revision) =>
                    await _con.QuerySingleAsync<TZ_tblWBTest_counters>(
                        @"SELECT * FROM TZ_tblWBtest_counters
    WHERE id_ssFam = @periodID AND NP = @partNo AND dbo.ufn_filter_revision(REV) = dbo.ufn_filter_revision(@revision);", new { periodID, partNo, revision }).ConfigureAwait(false);

        public async Task<long> CreatePickingCounterAsync(int id, string partNo, string revision) =>
                    await _con.ExecuteScalarAsync<long>("INSERT INTO TZ_TBLWBTEST_COUNTERS (ID_SSFAM,np,rev,counter) VALUES (@id,@partNo,@revision,0); SELECT SCOPE_IDENTITY();", new { id, partNo, revision }).ConfigureAwait(false);

        public async Task UpdatePickingCounterAsync(long id, int quantity, bool isActive, int sequenceNo) =>
                    await _con.ExecuteAsync("UPDATE TZ_TBLWBTEST_COUNTERs SET COUNTER=@quantity,is_active=@isActive, TOT_TEST = @sequenceNo WHERE id=@id", new { id, quantity, isActive, sequenceNo }).ConfigureAwait(false);

        public async Task<TZ_QCParam_App> GetContainerApprovalParamsAsync(string subFamily, string packType) =>
                    await _con.QuerySingleAsync<TZ_QCParam_App>("SELECT * FROM tz_qcparam_app WHERE familia=@subFamily AND PACK_TYPE=@packType;", new { subFamily, packType }).ConfigureAwait(false);

        public async Task<Tbl_qc_aproved_list?> TryGetApprovalByWorkOrderAsync(string workOrderCode) =>
                    await _con.QuerySingleAsync<Tbl_qc_aproved_list>("SELECT TOP 1 * from Tbl_qc_aproved_list WHERE codewo=@workOrderCode ORDER BY id DESC;", new { workOrderCode });

        public async Task<long> CreateApprovalAsync(string lineName, string partNo, string workOrderCode, int palletSize, string customerPartNo, string revision, int containerSize) =>
                    await _con.ExecuteScalarAsync<long>("INSERT INTO tbl_qc_aproved_list (linea,part_num, codewo, qty, fecha,pn_cliente,rev,qty_cont) VALUES (@lineName,@partNo,@workOrderCode,@palletSize,FORMAT(GETDATE(), 'dd-MMM-yyyy'),@customerPartNo,@revision,@containerSize); SELECT SCOPE_IDENTITY();",
                        new { lineName, partNo, workOrderCode, palletSize, customerPartNo, revision, containerSize }).ConfigureAwait(false);

        public async Task<IEnumerable<BomComponent>> GetBomAsync(string partNo, string revision) =>
                    await _con.QueryAsync<BomComponent>("SELECT CompNo, CompRev, CompDesc, PointOfUse FROM cegid.ufn_bom(@partNo, @revision);", new { partNo, revision }).ConfigureAwait(false);

        public async Task<IEnumerable<Etis_WB>> GetActiveSetAsync(string lineName) =>
                    await _con.QueryAsync<Etis_WB>("SELECT * FROM dbo.Etis_WB WITH(NOLOCK) WHERE status = 0 AND linea = @lineName;", new { lineName }).ConfigureAwait(false);

        public async Task<long> CreateMasterAsync(string clientName, string partNo, string revision, string customerPartNo, string partDescription, string destination, string po, string lotNo, string lineName, bool isClosed, int quantity, string masterType, bool wasPartial, bool isPartial, string julianDay, string productFamily, string workOrderCode, long? approvalID, string? approvalUser, DateTime? approvalDate) =>
                    await _con.ExecuteScalarAsync<long>(@"INSERT INTO Master_labels_WB (
    modelo, part_num, rev, cust_pn, description, customer, po_num, lotes, line, Closed, fecha, qty, master_type,was_partial,is_partial,juliano,familia,codew,aprov,aprov_user,aprov_date,hora)
VALUES (
    @clientName,@partNo,@revision,@customerPartNo,@partDescription,@destination,@po,@lotNo,@lineName,@isClosed,FORMAT(GETDATE(), 'dd-MMM-yyyy'),@quantity,@masterType,@wasPartial,@isPartial,@julianDay,@productFamily,@workOrderCode,@approvalID,@approvalUser,@approvalDate, FORMAT(GETDATE(), 'hh:mm tt'));
SELECT SCOPE_IDENTITY() as master_id;", new { clientName, partNo, revision, customerPartNo, partDescription, destination, po, lotNo, lineName, isClosed, quantity, masterType, wasPartial, isPartial, julianDay, productFamily, workOrderCode, approvalID, approvalUser, approvalDate }).ConfigureAwait(false);

        public async Task UpdateApprovalAsync(long approvalID, long masterID, int quantity, string julianDay) =>
                    await _con.ExecuteAsync("UPDATE TBL_QC_APROVED_LIST SET id_master=@masterID, lote=@julianDay, qty_final=@quantity WHERE id=@approvalID;", new { approvalID, masterID, quantity, julianDay }).ConfigureAwait(false);

        public async Task DeleteTemporaryPalletAsync(string lineName, string clientName, string partNo) =>
                    await _con.ExecuteAsync("DELETE FROM temp_pack_WB WHERE modelo=@clientName AND linea=@lineName AND num_p=@partNo;", new { partNo, clientName, lineName }).ConfigureAwait(false);

        public async Task CopyPackedUnitsToMasterAsync(long masterID, string partNo, string lineName) =>
                    await _con.ExecuteAsync(@"INSERT INTO dbo.Master_lbl_TMid (Master_id, TM_id, pack_time)
SELECT @masterID, TELESIS, fecha FROM TEMP_PACK_WB WHERE NUM_P=@partNo and LINEA=@lineName;", new { masterID, partNo, lineName }).ConfigureAwait(false);

        public async Task AddPickingUnitAsync(long unitID, string lineName, string partNo, int sequenceNo, string workOrderCode) =>
                    await _con.ExecuteAsync(
                        "INSERT INTO tbl_picking_wb (telesis_no,linea,fecha,np_final,type,codew) values (@unitID,@lineName,FORMAT(GETDATE(), 'dd-MMM-yyyy'),@partNo,@sequenceNo + 1,@workOrderCode)",
                        new { unitID = unitID.ToString(), lineName, partNo, sequenceNo, workOrderCode }).ConfigureAwait(false);
        public async Task AddTracedUnitAsync(long unitID, string lineName, int operation, string clientName, string partNo, string workOrderCode) =>
            await _con.ExecuteAsync(@"INSERT INTO Trazab_WB (Telesis_no, Linea, no_empleado, fecha_scan, hora_scan,ETI_no,MODELO,NP_FINAL,codew)
VALUES (@unitID,@lineName,@operation,FORMAT(GETDATE(), 'dd-MMM-yyyy'),Format(GETDATE(), 'hh:mm tt'),(SELECT TOP 1 SET_ID FROM dbo.Etis_WB WITH(NOLOCK) WHERE linea=@lineName AND status=0),@clientName,@partNo,@workOrderCode)",
                new { unitID = unitID.ToString(), lineName, operation, clientName, partNo, workOrderCode }).ConfigureAwait(false);

        /// <summary>
        /// Pack unit into temporary pallet and update work order production counter.
        /// </summary>
        /// <param name="lineName">Line name (ie WB LA).</param>
        /// <param name="clientName"></param>
        /// <param name="unitID"></param>
        /// <param name="partNo"></param>
        /// <param name="julianDay"></param>
        /// <param name="isPartial"></param>
        /// <param name="masterID"></param>
        /// <param name="approvalID"></param>
        /// <param name="workOrderCode"></param>
        /// <param name="lineID"></param>
        /// <returns></returns>
        public async Task AddPackedUnitAsync(string lineName, string clientName, long unitID, string partNo, string julianDay, bool isPartial, long? masterID, long? approvalID, string workOrderCode, int lineID) =>
                    await _con.ExecuteAsync(@"
INSERT INTO temp_pack_WB (linea, modelo, telesis,num_p,LEVEL,is_partial,master_id,Aproved) VALUES (@lineName,@clientName,@unitID,@partNo,@julianDay,@isPartial,@masterID,@approvalID);
UPDATE APPS.dbo.pro_production SET current_qty = current_qty + 1 WHERE codew=@workOrderCode AND id_line = @lineID;",
                        new { lineName, clientName, unitID = unitID.ToString(), partNo, julianDay, isPartial, masterID = masterID.ToString(), approvalID, workOrderCode, lineID }).ConfigureAwait(false);

        public async Task UpdateProduction() =>
            await _con.ExecuteAsync("").ConfigureAwait(false);

        public async Task<IEnumerable<HourlyProductionEntry>> GetHourlyProductionAsync(string lineCode) =>
            await _con.QueryAsync<HourlyProductionEntry>("EXEC dbo.usp_get_line_hourly_production @lineCode;", new { lineCode })
            .ConfigureAwait(false);

        public async Task<long?> GetLastMasterFolioByLineAsync(string lineName) =>
            await _con.ExecuteScalarAsync<long?>("SELECT MAX(id) [Folio] FROM dbo.Master_labels_WB WHERE line=@lineName;", new { lineName });

        public async Task<string> GetOrigenByCegid(string partNo, string partRev)
        {
            string origen = await _con.QueryFirstAsync<string>(
                //"SELECT ISNULL(NULLIF(RTRIM((SELECT * FROM [dbo].[ufn_GetArticleOrigenCegid] (@partNo,@PartRev))), ' '), ' ') as [Origen]",
                // "SELECT * FROM [dbo].[ufn_GetArticleOrigenCegid] (@partNo,@partRev)",
                "SELECT top 1 Origen FROM dbo.ufn_GetArticleOrigenCegid (@partNo, @partRev)",
                new { partNo, partRev });
            return origen;
        }

    }
}
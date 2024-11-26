using GT.Trace.Packaging.Infra.DataSources.Entities;

namespace GT.Trace.Packaging.Infra.DataSources
{
    public class GttSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public GttSqlDB(ConfigurationSqlDbConnection<GttSqlDB> con)
        {
            _con = con;
        }

        public async Task<LineShiftHeadcount> GetLineHeadcountAsync(string lineCode) =>
            await _con.QuerySingleAsync<LineShiftHeadcount>("SELECT * FROM dbo.UfnLineShiftHeadcount(@lineCode);", new { lineCode }).ConfigureAwait(false);

        public async Task<IEnumerable<ProductionTimeInterval>> GetTimeIntervalProduction(string lineCode, int hourlyRate) =>
            await _con.QueryAsync<ProductionTimeInterval>(
                "SELECT * FROM dbo.UfnProductionTimeIntervals(@lineCode, NULL, @hourlyRate);",
                new { lineCode, hourlyRate }).ConfigureAwait(false);

        /// <summary>
        /// Add the packaging process to the unit's process history.
        /// </summary>
        /// <remarks>The value 999 is the packaging process ID.</remarks>
        /// <param name="unitID">The unit's ID.</param>
        /// <param name="lineCode">Two-character line code.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RecordPackagingProcessHistoryAsync(long unitID, string lineCode) =>
            await _con.ExecuteAsync("INSERT INTO dbo.ProcessHistory (UnitID, ProcessID, LineCode) VALUES(@unitID, '999', @lineCode);", new { unitID, lineCode })
            .ConfigureAwait(false);

        public async Task RecordUnitIDShapshotHistoryAsync(long unitID, string lineCode) =>
           await _con.ExecuteAsync("EXECUTE [dbo].[UpsInsertUnitIDShapshotHistory] @lineCode,@unitID;", new { unitID, lineCode })
           .ConfigureAwait(false);

        /// <summary>
        /// Record production.
        /// </summary>
        /// <param name="lineCode">The line code.</param>
        /// <param name="partNo">The model's part number.</param>
        /// <param name="revision">The model's revision.</param>
        /// <param name="workOrderCode">The work order code.</param>
        /// <param name="quantity">The produced quantity.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RecordProductionAsync(string lineCode, string partNo, string revision, string workOrderCode, int quantity) =>
            await _con.ExecuteAsync(
                "INSERT INTO ProductionLogs (LineCode, PartNo, Revision, WorkOrderCode, Quantity) VALUES(@lineCode, @partNo, @revision, @workOrderCode, @quantity);",
                new { lineCode, partNo, revision, workOrderCode, quantity })
            .ConfigureAwait(false);

        public async Task<string> GetActiveWorkOrderAsync(string lineCode) =>
            await _con.ExecuteScalarAsync<string>(
                "SELECT WorkOrderCode FROM dbo.LineProductionSchedule WHERE LineCode = @lineCode AND UtcEffectiveTime <= GETUTCDATE() AND UtcExpirationTime > GETUTCDATE();",
                new { lineCode }).ConfigureAwait(false);

        
        //Agregado para validar que el dato de la orden este en la tabla LineProductionSchedule ya que si el dato no esta ahi, se pierde la trazabilidad RA: 09/20/2023
        #region Agregado para correguir Bug de trazabilidad
        /*Esto se agrego debido a que por algun extraño Bug no se guardaba la informacion en esta tabla, la cual es de alta importancia para obtener la trazabilidad, sin la informacion en esta tabla la trazabilidad
         no apareceria, lo cual es medianamente critico, ya que si se agrega el dato en el rango de tiempo correcto, la trazabilidad volveria a aparecer, esto afectaba a todas las lineas de manera aleatoria en modelos aleatorios
        con esta correccion ya no deberia de suceder eso.*/
        public async Task<string> LineProductionScheduleAsync(string lineCode, string workOrderCode, string partNo) =>
            await _con.QueryFirstAsync<string>("SELECT TOP (1) WorkOrderCode FROM [gtt].[dbo].[LineProductionSchedule] where LineCode = @lineCode AND WorkOrderCode = @workOrderCode AND PartNo = @partNo AND UtcEffectiveTime <= UtcEffectiveTime and UtcExpirationTime >= GETUTCDATE()",
                new { lineCode, workOrderCode, partNo }).ConfigureAwait(false);

        public async Task UpdateUtcExpirationTimeAsync(string linecode) =>
            await _con.ExecuteAsync("UPDATE [gtt].[dbo].[LineProductionSchedule] SET UtcExpirationTime = GETUTCDATE() WHERE LineCode = @linecode and UtcExpirationTime > GETUTCDATE()", new { linecode }).ConfigureAwait(false);
 
        public async Task RecordProductionNewAsync(string lineCode, string workOrderCode, string partNo, string revision) =>
            await _con.ExecuteAsync(
                "INSERT INTO LineProductionSchedule (LineCode,WorkOrderCode,PartNo,HourlyRate,UtcEffectiveTime,Revision) VALUES (@lineCode,@workOrderCode,@partNo,1,GETUTCDATE(),@revision)",
                //"INSERT INTO LineProductionSchedule (LineCode,WorkOrderCode,PartNo,HourlyRate,UtcEffectiveTime,Revision) values (@lineCode,@workOrderCode,@partNo,ISNULL((SELECT TOP 1 HourlyRate FROM LineProductionSchedule WHERE LineCode = @lineCode AND PartNo = @partNo ORDER BY UtcExpirationTime DESC),0),GETUTCDATE(),@revision)",
                new { lineCode, workOrderCode, partNo, revision }).ConfigureAwait(false);
        #endregion


        public async Task<IEnumerable<PointOfUseEtis>> GetActiveSetByLineAsync(string lineCode) =>
            await _con.QueryAsync<PointOfUseEtis>(@"SELECT e.* FROM dbo.LinePointsOfUse p
            JOIN dbo.PointOfUseEtis e
                ON e.PointOfUseCode = p.PointOfUseCode AND e.UtcUsageTime IS NOT NULL AND e.UtcExpirationTime IS NULL
            WHERE LineCode = @lineCode;", new { lineCode }).ConfigureAwait(false);

        internal async Task<IEnumerable<LineRouting>> GetLineRoutingByLineCode(string lineCode) =>
            await _con.QueryAsync<LineRouting>("SELECT * FROM LineRouting WHERE LineCode=@lineCode;", new { lineCode }).ConfigureAwait(false);

        //Se agrego para obtener el ultimo mensaje de la linea, se piensa usar para el bloqueo de linea, que de la info y se imprima en la pantalla
        public async Task<string> GetMessageFromAssembly(string linecode) =>
            await _con.ExecuteScalarAsync<string>("SELECT top 1 ClientMessage FROM [gtt].[dbo].[EventsHistory] where " +
                "ClientMessage like ('%corresponde con ningún punto de uso para la gama%') and LineCode = @linecode order BY UtcTimeStamp desc", new { linecode }).ConfigureAwait(false);

        #region Correccion de BUG
        //agregado para "Corregir" problema de 2 ordenes activas en una misma linea RA: 06/15/2023
        //Se corrigio este bug con el metodo Updateproductionscheduling, cuando hay 2 ordenes activas en automatico quita la orden repetida y deja la que deberia de estar activa en GT-APP
        public async Task<int> CountProductionScheduleAsync(string lineCode) =>
                                    await _con.ExecuteScalarAsync<int>("SELECT COUNT(WorkOrderCode) AS [CountWorkOrderCode] FROM LineProductionSchedule WHERE LineCode = @lineCode AND UtcExpirationTime >= GETUTCDATE()",
            //await _con.ExecuteScalarAsync<int>("SELECT COUNT(WorkOrderCode) AS [CountWorkOrderCode] FROM LineProductionSchedule WHERE LineCode = @lineCode AND UtcExpirationTime >= '2099-12-31 23:59:00.000'",
            //await _con.ExecuteScalarAsync<int>("SELECT COUNT(WorkOrderCode) AS [CountWorkOrderCode] FROM LineProductionSchedule WHERE LineCode = @LineCode AND UtcExpirationTime >= GETUTCDATE();" +
            //    "UPDATE LineProductionSchedule SET UtcExpirationTime = GETUTCDATE() WHERE LineCode = @LineCode AND UtcExpirationTime >= GETUTCDATE() AND WorkOrderCode != @workOrderCode;",
            new { lineCode }).ConfigureAwait(false);
        public async Task Updateproductionscheduling(string lineCode, string workOrderCode) =>
            await _con.ExecuteAsync("UPDATE LineProductionSchedule SET UtcExpirationTime = GETUTCDATE() WHERE LineCode = @lineCode AND UtcExpirationTime >= GETUTCDATE() AND WorkOrderCode != @workOrderCode", new { lineCode, workOrderCode }).ConfigureAwait(false);
        #endregion

        #region Lineas Motores China MW,MX
        public async Task  AddMotorsData(string SerialNumber, string modelo, string Volt, string RPM, DateTime DateTimeMotor, string Rev) =>
            await _con.ExecuteScalarAsync<long>("INSERT INTO MotorsData (modelo,SerialNumber,Volt,RPM,DateTimeMotor,Rev) VALUES (@modelo,@SerialNumber,@Volt,@RPM,@DateTimeMotor,@Rev)", new {SerialNumber,modelo,Volt,RPM,DateTimeMotor,Rev }).ConfigureAwait(false);

        #endregion

        //Agregados especiales para EZ
        #region EZ
        //Agregado para insertar Motor, en la linea Frameless. aun no utilizado pero se va a usar.
        public async Task<string> InsertInfoMotorsData(string SerialNumber, string Volt, string RPM, DateTime DateTimeMotor, string LineCode) =>
            await _con.ExecuteScalarAsync<string>("INSERT INTO dbo.MotorsData([SerialNumber],[Volt],[RPM],[DateTimeMotor],[LineCode])VALUES(@SerialNumber,@Volt,@RPM,@DateTimeMotor,@LineCode);", 
                new { SerialNumber, Volt, RPM, DateTimeMotor, LineCode }).ConfigureAwait(false);

        //Agregado para EZ Join RA: 6/28/2023
        
        public async Task<int> EZRegisteredInformation(long unitID, string Date1, string Time1, string Motor_Number1, string Date2, string Time2, string Motor_Number2)=>
            await _con.QuerySingleAsync<int>("SELECT CASE WHEN (SELECT COUNT(UnitID) FROM EZ2000Motors WHERE UnitID = @UnitID AND isEnable = 1 OR [Date] = @Date1 AND [Time] = @Time1 AND Motor_number = @Motor_Number1 AND isEnable = 1 OR [Date] = @Date2 AND [Time] = @Time2 AND Motor_number = @Motor_Number2 AND isEnable = 1) > 0 THEN 1 ELSE 0 END AS Result"
                , new { unitID,Date1,Time1,Motor_Number1, Date2, Time2, Motor_Number2 }).ConfigureAwait(false);

        public async Task<int> EZMotorDataRegisteredInformation(string Motor_Number, DateTime DateTimeMotor) =>
            await _con.QuerySingleAsync<int>("SELECT Result FROM dbo.UfnEnginePinionSubassemblyRatioCompatibility(@Motor_Number, @DateTimeMotor);",
                new { Motor_Number,DateTimeMotor}).ConfigureAwait(false);

        public async Task<int> EZRegisteredInformation(string Motor_Number, DateTime DateTimeMotor)=> //Pokayoke agregado el 2/28/2024
            await _con.QuerySingleAsync<int>("SELECT COUNT(ID) AS [Motor] FROM [gtt].[dbo].[MotorsData] WHERE DateTimeMotor is not null AND DateTimeMotor = @DateTimeMotor AND SerialNumber = @Motor_Number;", 
                new { Motor_Number, DateTimeMotor }).ConfigureAwait(false);
        
        //NOTE: CAMBIAR ESTO DE AQUI A LA DB APPS PARA OPTIMIZAR ESTE PEDO 03/04/2024
        public async Task<string> GetEZModel()=>
            await _con.QuerySingleAsync<string>("SELECT TOP (1) RTRIM([part_number]) FROM [MXSRVTRACA].[APPS].[dbo].[pro_production] WHERE is_stoped = 0 AND is_running = 1 and is_finished = 0 AND id_line = 5").ConfigureAwait(false);


        public async Task AddEZJoinMotors(long unitID, string Web1, string Current1, string Speed1, string Date1, string Time1, string Motor_Number1, string PN1, string AEM1, string Rev1, string Web2, string Current2, string Speed2, string Date2, string Time2, string Motor_Number2, string PN2, string AEM2, string Rev2) =>
           await _con.ExecuteAsync("INSERT INTO [dbo].[EZ2000Motors]([UnitID],[Website],[No_Load_Current],[No_Load_Speed],[Date],[Time],[Motor_number],[PN],[AEM],[Rev])VALUES(@unitID,@Web1,@Current1,@Speed1,@Date1,@Time1,@Motor_Number1,@PN1,@AEM1,@Rev1),(@unitID,@Web2,@Current2,@Speed2,@Date2,@Time2,@Motor_Number2,@PN2,@AEM2,@Rev2)",
               new { unitID, Web1, Current1, Speed1, Date1, Time1, Motor_Number1,PN1,AEM1,Rev1, Web2, Current2, Speed2, Date2,Time2,Motor_Number2,PN2,AEM2,Rev2}).ConfigureAwait(false);

        public async Task AddEZJoinMotors(long unitID, string Web, string Current, string Speed, string Date, string Time, string Motor_Number, string PN, string AEM, string Rev) =>
            await _con.ExecuteAsync("INSERT INTO [dbo].[EZ2000Motors]([UnitID],[Website],[No_Load_Current],[No_Load_Speed],[Date],[Time],[Motor_number],[PN],[AEM],[Rev])VALUES(@unitID,@Web,@Current,@Speed,@Date,@Time,@Motor_Number,@PN,@AEM,@Rev)",
                new { unitID, Web, Current, Speed, Date, Time, Motor_Number, PN, AEM, Rev }).ConfigureAwait(false);

        public async Task DelJoinEZMotors(long unitID)=>
            await _con.ExecuteAsync("UPDATE EZ2000Motors SET isEnable = 0 WHERE UnitID = @unitID", 
                new { unitID }).ConfigureAwait(false);

        public async Task AddPalletQR(long UnitID, string PalletID, string LineCode)
            => await _con.ExecuteAsync("INSERT INTO [dbo].[LinePallet]([UnitID],[PalletID],[LineCode])VALUES(@UnitID,@PalletID,@LineCode)",
                new {UnitID,PalletID,LineCode}).ConfigureAwait(false);

        public async Task<int> PalletRegisteredInformation(long UnitID)=>
            await _con.ExecuteScalarAsync<int>("SELECT COUNT([UnitID]) FROM [gtt].[dbo].[LinePallet] WHERE UnitID = @UnitID",
                new {UnitID}).ConfigureAwait(false);
        #endregion

        //Agregado para LP + Frameless Join RA: 6/23/2023
        #region LP + Frameless 
        public async Task AddJoinFramelessMotors(long unitID, string componentID, string lineCode, string partNo) => await _con.ExecuteAsync("INSERT INTO [dbo].[ComponentJoining] ([UnitID],[ComponentID],[LineCode],[PartNo])VALUES(@unitID,@componentID,@lineCode,@partNo)",
               new { unitID, componentID, lineCode, partNo }).ConfigureAwait(false);
        public async Task DelJoinFramelessMotors(long unitID, string componentID) => await _con.ExecuteAsync("UPDATE ComponentJoining SET isEnable = 0 WHERE UnitID = @unitID AND ComponentID = @componentID",
               new { unitID, componentID}).ConfigureAwait(false);

        public async Task<int> FramelessRegisteredInformation(long unitID, string componentID)
        {
            return await _con.ExecuteScalarAsync<int>("SELECT CASE WHEN ((SELECT COUNT(UnitID) FROM ComponentJoining WHERE UnitID = @unitID AND isEnable = 1) + (SELECT COUNT(ComponentID) FROM ComponentJoining WHERE ComponentID = @componentID AND isEnable = 1)) > 0 THEN 1 ELSE 0 END AS Result"
                 , new { unitID, componentID }).ConfigureAwait(false);
        }
        public async Task<int> FramelessRegisteredInformationUnitID(long unitID)
        {
            return await _con.ExecuteScalarAsync<int>("SELECT COUNT(UnitID) FROM ComponentJoining WHERE UnitID = @unitID AND isEnable = 1"
                 , new { unitID }).ConfigureAwait(false);
        }
        public async Task<int> FramelessRegisteredInformationComponentID(string componentID)
        {
            return await _con.ExecuteScalarAsync<int>("SELECT COUNT(ComponentID) FROM ComponentJoining WHERE ComponentID = @componentID AND isEnable = 1"
                 , new { componentID }).ConfigureAwait(false);
        }
        #endregion

        #region Piñones Motores EZ

        public async Task AddEZMotorsData(string model, string serialNumber, string Volt, string RPM, DateTime DateTimeMotor, string Rev, string lineCode, string pinionPartNum,string motorPartNum)
        {
            //await _con.ExecuteAsync("INSERT INTO MotorsData (Modelo,SerialNumber,LineCode,PinionPartNum,MotorPartNum) VALUES (@model,@serialNumber,@lineCode,@pinionPartNum,@motorPartNum)", new {model,serialNumber, lineCode,pinionPartNum,motorPartNum}).ConfigureAwait(false);
            await _con.ExecuteAsync("INSERT INTO [gtt].[dbo].[MotorsData] ([Modelo],[SerialNumber],[Volt],[RPM],[DateTimeMotor],[Rev],[LineCode],[PinionPartNum],[MotorPartNum]) " +
                "VALUES (@model,@serialNumber,@Volt,@RPM,@DateTimeMotor,@Rev,@lineCode,@pinionPartNum,@motorPartNum);", 
                new { model, serialNumber, Volt, RPM, DateTimeMotor, Rev, lineCode, pinionPartNum, motorPartNum }).ConfigureAwait(false);
            //INSERT INTO MotorsData (Modelo,SerialNumber,LineCode) VALUES (@model,@serialNumber,@lineCode)
        }
        public async Task<int> GetEzMotorsData(string model, string serialNumber, string lineCode, DateTime DateTimeMotor)
        {
            return await _con.QuerySingleAsync<int>("SELECT COUNT([ID]) FROM [gtt].[dbo].[MotorsData] WHERE Modelo = @model AND SerialNumber = @serialNumber AND LineCode = @lineCode AND DateTimeMotor = @DateTimeMotor", new { model, serialNumber,lineCode, DateTimeMotor });
        }

        #endregion

        #region EZ
        /*Nuevo para EZ 
         Candados que falta en el Sistema de Traza
        Correo de Fabien Gurrier Lun 2023-12-04 7:53 AM
        */
        public async Task<int> GetProcessHistory(long unitID) =>
            await _con.QueryFirstAsync<int>("SELECT COUNT([UnitID]) FROM [gtt].[dbo].[ProcessHistory] WHERE LineCode = 'LE' AND ProcessID = 0 AND UnitID = @unitID", new { unitID }).ConfigureAwait(false);
        #endregion

        /// <summary>
        /// Delete UnitID from GT-System in Database:
        /// GTT - ProcessHistory
        /// TRAZAB - Temp_pack_WB
        /// Update current_qty -1 in Database: 
        /// APPS - pro_production
        /// </summary>
        /// <param name="lineName"> Example "WB LO"</param>
        /// <param name="unitID"> This is obtained through a method that parses a regular expression and returns the scanInput data
        /// Example from scan Input: " [)>06SWB10725151PAUC15714ZGT1TGT871402TB23T12824 " expected value of the method for this variable 10725151 </param>
        /// <param name="workOrderCode">Example "W08411211"  </param>
        /// <param name="lineID"> Example: 5 </param>
        /// <param name="lineCode">Example: LO </param>
        /// <returns></returns>
        public async Task UnpackedUnitAsync(string lineName, long unitID, string workOrderCode, string lineCode) =>
            await _con.ExecuteAsync(@"  DELETE FROM [gtt].[dbo].[ProcessHistory] WHERE LineCode = @lineCode AND UnitID = @unitID;
                                        DELETE FROM [mxsrvtraca].[TRAZAB].[dbo].[Temp_pack_WB] WHERE linea = @lineName AND telesis = @unitID;
                                        UPDATE mxsrvtraca.APPS.dbo.pro_production SET current_qty = current_qty - 1 WHERE codew=@workOrderCode AND id_line = (SELECT id FROM mxsrvtraca.[APPS].[dbo].[pro_prod_units] WHERE comments = @lineName);",
        new { lineName, unitID, workOrderCode, lineCode }).ConfigureAwait(false);
        /// <summary>
        /// Esta consulta hace lo siguiente:
        /// Filtra los registros donde LineCode es 'LH' y PartNo es 'AAA' o '*'.
        /// Usa ORDER BY con una cláusula CASE para priorizar los registros donde PartNo es 'AAA'. Si no hay registros con PartNo 'AAA', entonces selecciona los registros con PartNo '*', el * signficia que tomara todos los modelos que corran en esta linea como necesarios para validar la prueba funcional. 
        /// Si necesitas manejar valores NULL específicamente, puedes ajustar la consulta para incluir COALESCE o IS NULL según sea necesario.
        /// </summary>
        /// <param name="lineCode"></param>
        /// <returns></returns>
        public async Task<LineModelFunctionalTest?> ValidationDataForFunctionalTestingByModelAndLine(string lineCode, string PartNo)=>
            await _con.QuerySingleAsync<LineModelFunctionalTest?>("SELECT TOP (1) [LineCode], [PartNo]" +
                " FROM [gtt].[dbo].[LineModelFunctionalTest]" +
                " WHERE LineCode = @lineCode " +
                " AND (PartNo = @PartNo OR PartNo = '*')" +
                " ORDER BY CASE" +
                " WHEN PartNo = @PartNo THEN 1" +
                " ELSE 2" +
                " END;", new { lineCode , PartNo }).ConfigureAwait(false);
    }
}
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
        public async Task<LineProductionSchedule> LineProductionScheduleAsync(string lineCode, string workOrderCode, string partNo) =>
            await _con.QuerySingleAsync<LineProductionSchedule>("SELECT TOP (1) * FROM [gtt].[dbo].[LineProductionSchedule] where LineCode = @lineCode AND WorkOrderCode = @workOrderCode AND PartNo = @partNo and UtcExpirationTime >= GETUTCDATE() order by UtcExpirationTime DESC", 
                new {lineCode,workOrderCode,partNo}).ConfigureAwait(false);
        public async Task RecordProductionNewAsync(string lineCode, string workOrderCode, string partNo, string revision) =>
            await _con.ExecuteAsync("INSERT INTO LineProductionSchedule (LineCode,WorkOrderCode,PartNo,HourlyRate,UtcEffectiveTime,Revision) values (@lineCode,@workOrderCode,@partNo,ISNULL((SELECT TOP 1 HourlyRate FROM LineProductionSchedule WHERE LineCode = @lineCode AND PartNo = @partNo ORDER BY UtcExpirationTime DESC),0),GETUTCDATE(),@revision)",
                new {lineCode,workOrderCode,partNo,revision}).ConfigureAwait(false);
        #endregion

        public async Task<IEnumerable<PointOfUseEtis>> GetActiveSetByLineAsync(string lineCode) =>
            await _con.QueryAsync<PointOfUseEtis>(@"SELECT e.* FROM dbo.LinePointsOfUse p
            JOIN dbo.PointOfUseEtis e
                ON e.PointOfUseCode = p.PointOfUseCode AND e.UtcUsageTime IS NOT NULL AND e.UtcExpirationTime IS NULL
            WHERE LineCode = @lineCode;", new { lineCode }).ConfigureAwait(false);

        internal async Task<IEnumerable<LineRouting>> GetLineRoutingByLineCode(string lineCode) =>
            await _con.QueryAsync<LineRouting>("SELECT * FROM LineRouting WHERE LineCode=@lineCode;", new { lineCode }).ConfigureAwait(false);

        //Esto es nuevo para la trazabilidad
        //Actualmente esto nunca se uso pero lo dejare por si acaso. RA: 5/22/2023
        public async Task AddTracedUnitAsync(long unitID, string partNo, string lineCode, string workOrderCode) =>
            await _con.ExecuteAsync("EXEC UpsInsertProductionTraceability @unitID, @partNo, @lineCode, @workOrderCode;", new { unitID, partNo , lineCode, workOrderCode }).ConfigureAwait(false);


        //Se agrego para obtener el ultimo mensaje de la linea, se piensa usar para el bloqueo de linea, que de la info y se imprima en la pantalla
        public async Task<string> GetMessageFromAssembly(string linecode) =>
            await _con.ExecuteScalarAsync<string>("SELECT top 1 ClientMessage FROM [gtt].[dbo].[EventsHistory] where " +
                "ClientMessage like ('%corresponde con ningún punto de uso para la gama%') and LineCode = @linecode order BY UtcTimeStamp desc", new { linecode }).ConfigureAwait(false);
        //public async Task<string> GetMessageFromAssembly(string linecode) =>
        //    await _con.ExecuteScalarAsync<string>("SELECT top 1 ClientMessage FROM [gtt].[dbo].[EventsHistory] where LineCode = @linecode order BY UtcTimeStamp desc", new { linecode }).ConfigureAwait(false);

        //agregado para "Corregir" problema de 2 ordenes activas en una misma linea RA: 06/15/2023
        public async Task<int> CountProductionScheduleAsync(string LineCode) =>
            await _con.ExecuteScalarAsync<int>("SELECT COUNT(WorkOrderCode) AS [CountWorkOrderCode] FROM LineProductionSchedule WHERE LineCode = @LineCode AND UtcExpirationTime >= '2099-12-31 23:59:00.000'",
                new { LineCode }).ConfigureAwait(false);

        //Agregados especiales para EZ
        #region EZ
        //Agregado para insertar Motor, en la linea Frameless. aun no utilizado pero se va a usar.
        public async Task<string> InsertInfoMotorsData(string SerialNumber, string Volt, string RPM, DateTime DateTimeMotor, string LineCode) =>
            await _con.ExecuteScalarAsync<string>("INSERT INTO dbo.MotorsData([SerialNumber],[Volt],[RPM],[DateTimeMotor],[Line])VALUES(@SerialNumber,@Volt,@RPM,@DateTimeMotor,@LineCode);", 
                new { SerialNumber, Volt, RPM, DateTimeMotor, LineCode }).ConfigureAwait(false);

       

        ////Agregado para evitar que las lineas inicien si el numero de componentes de las gamas en cegid y en trazab no coinciden
        //public async Task<bool> CountComponentsBomAsync(string partNo, string linecode)=>
        //    //await _con.ExecuteScalarAsync<bool>("SELECT dbo.CountComponentsBom(@partNo, @linecode) AS [CountComponentsBom];" se comento esto para dejarlo de usar ya que la consulta de abajo es mejor RA: 07/05/2023
        //    await _con.ExecuteScalarAsync<bool>("SELECT dbo.UfnGetDifferenceCountDataBoom(@partNo, @linecode) AS [CountComponentsBom];"
        //        , new { partNo, linecode}).ConfigureAwait(false);

        //Agregado para EZ Join RA: 6/28/2023
        
        public async Task<int> EZRegisteredInformation(long unitID, string Date1, string Time1, string Motor_Number1, string Date2, string Time2, string Motor_Number2)=>
            await _con.ExecuteScalarAsync<int>("SELECT CASE WHEN (SELECT COUNT(UnitID) FROM EZ2000Motors WHERE UnitID = @UnitID AND isEnable = 1 OR [Date] = @Date1 AND [Time] = @Time1 AND Motor_number = @Motor_Number1 AND isEnable = 1 OR [Date] = @Date2 AND [Time] = @Time2 AND Motor_number = @Motor_Number2 AND isEnable = 1) > 0 THEN 1 ELSE 0 END AS Result"
                , new { unitID,Date1,Time1,Motor_Number1, Date2, Time2, Motor_Number2 }).ConfigureAwait(false);
        public async Task AddEZJoinMotors(long unitID, string Web1, string Current1, string Speed1, string Date1, string Time1, string Motor_Number1, string PN1, string AEM1, string Rev1, string Web2, string Current2, string Speed2, string Date2, string Time2, string Motor_Number2, string PN2, string AEM2, string Rev2) =>
           await _con.ExecuteAsync("INSERT INTO [dbo].[EZ2000Motors]([UnitID],[Website],[No_Load_Current],[No_Load_Speed],[Date],[Time],[Motor_number],[PN],[AEM],[Rev])VALUES(@unitID,@Web1,@Current1,@Speed1,@Date1,@Time1,@Motor_Number1,@PN1,@AEM1,@Rev1),(@unitID,@Web2,@Current2,@Speed2,@Date2,@Time2,@Motor_Number2,@PN2,@AEM2,@Rev2)",
               new { unitID, Web1, Current1, Speed1, Date1, Time1, Motor_Number1,PN1,AEM1,Rev1, Web2, Current2, Speed2, Date2,Time2,Motor_Number2,PN2,AEM2,Rev2}).ConfigureAwait(false);

        public async Task DelJoinEZMotors(long unitID)=>
            await _con.ExecuteAsync("UPDATE EZ2000Motors SET isEnable = 0 WHERE UnitID = @unitID", new { unitID }).ConfigureAwait(false);

        public async Task AddPalletQR(long UnitID, string PalletID, string LineCode)
            => await _con.ExecuteAsync("INSERT INTO [dbo].[LinePallet]([UnitID],[PalletID],[LineCode])VALUES(@UnitID,@PalletID,@LineCode)",new {UnitID,PalletID,LineCode}).ConfigureAwait(false);
        public async Task<int> PalletRegisteredInformation(long UnitID)=>
            await _con.ExecuteScalarAsync<int>("SELECT COUNT([UnitID]) FROM [gtt].[dbo].[LinePallet] WHERE UnitID = @UnitID", new {UnitID}).ConfigureAwait(false);
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
    }
}
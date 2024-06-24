using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GT.Trace.ProcessHistory.HttpServices.EndPoints.Units.Lines.Processes
{
    //Hoy estoy en modo huevon y no quiero hacer una API solo para hacer un test ya que realmente tuve un mal dia com mi Laptop que no fue nada agradable.
    //RA: 05/10/2023
    #region

    public record Label(long UnitID);
    public record MotorData(string Volt, string RPM, DateTime DateTime, string SerialNumber);

    public static class Labels
    {
        public const string InformationSeparatorThree = "\u001d";
        public const string EndOfTransmission = "\u0004";
        private static string WalkBehindLabelFormatRegExPattern => $"\\[\\)>06SWB(?<transmissionID>\\d+)P(?<clientPartNo>.+)Z.+1T(?<partNo>.+)2T(?<partRev>.+)3T(?<julianDay>\\d+)$";
        private static string ClearInputFromSpecialCharacters(string input) => input.Replace(InformationSeparatorThree, "").Replace(EndOfTransmission, "");

        public static Label? TryParseNewWBFormat(string value)
        {
            var match = Regex.Match(
                ClearInputFromSpecialCharacters(value),
                WalkBehindLabelFormatRegExPattern,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                return new Label(
                long.Parse(match.Groups["transmissionID"].Value));
            }
            else
            {
                return null;
            }
        }

        public static MotorData? ParseMotorData(string input)
        {
            var regex = new Regex(@"^(?<Volt>[0-9\.]+[A-Z])\|(?<RPM>[0-9]+)\|(?<datetime>\d{6}-\d{6})\|(?<SerialNumber>[0-9A-Z]+)$");
            var match = regex.Match(input);
            if (!match.Success) return null;
            var dateTimeString = match.Groups["datetime"].Value;
            var dateTime = DateTime.ParseExact(dateTimeString, "yyMMdd-HHmmss", CultureInfo.InvariantCulture);

            return new MotorData(
                match.Groups["Volt"].Value,
                match.Groups["RPM"].Value,
                dateTime,
                match.Groups["SerialNumber"].Value
            );
        }
    }
    #endregion
#pragma warning disable CS8603 // Possible null reference return.
    [ApiController]
    public class RecordProcessEndPoint : ControllerBase
    {
        private readonly ILogger<RecordProcessEndPoint> _logger;

        public RecordProcessEndPoint(ILogger<RecordProcessEndPoint> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/api/units")]
        public async Task<IActionResult> Execute2([FromQuery] long unitID, [FromQuery] string lineCode, [FromQuery] string processNo)
        {
            return await Execute(unitID, lineCode, processNo);
        }

        [HttpPost]
        [Route("/api/units/{unitID}/lines/{lineCode}/processes/{processNo}")]
        public async Task<IActionResult> Execute([FromRoute] long unitID, [FromRoute] string lineCode, [FromRoute] string processNo)
        {
            var currentUnitsProcess = await GetUnitsCurrentProcess(unitID, lineCode).ConfigureAwait(false);
            if (string.IsNullOrEmpty(currentUnitsProcess))
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponse($"Unidad #{unitID} no encontrada en la linea {lineCode}."));
            }

            if (!await RouteTransitionIsValid(lineCode, currentUnitsProcess, processNo).ConfigureAwait(false))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse($"Unidad #{unitID} se encuentra en el proceso \"{currentUnitsProcess}\" el cual no es aceptable en el proceso \"{processNo}\" o el proceso \"{processNo}\" no se encuentra definido para la linea \"{lineCode}\"."));
            }

            await RecordProcess(unitID, processNo, lineCode).ConfigureAwait(false);

            await UpdateUnitsCurrentProcess(unitID, processNo).ConfigureAwait(false);

            return Ok(new ApiResponse("OK"));
        }

        //Hoy estoy en modo huevon y no quiero hacer una API solo para hacer un test ya que realmente tuve un mal dia com mi Laptop que no fue nada agradable.
        //RA: 05/10/2023
        //---------------
        #region
        [HttpPost]
        [Route("/api/eti/{EtiNo}")]
        public async Task<IActionResult> Execute3([FromRoute] string EtiNo)
        {

            await UpdateEtiNoUtcUsageTime(EtiNo).ConfigureAwait(false);
            return Ok(new ApiResponse("OK"));
        }

        private async Task UpdateEtiNoUtcUsageTime(string EtiNo)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            await con.ExecuteAsync(
                "UPDATE PointOfUseEtisV2 SET UtcUsageTime = GETUTCDATE() WHERE EtiNo = @EtiNo AND UtcExpirationTime is NULL and UtcUsageTime is not NULL and IsDepleted != 1",
                new { EtiNo });
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        [Route("/api/UnitID/{ezLabel}/Line/{LineCode}")]
        public async Task<IActionResult> Execute([FromRoute] string ezLabel, [FromRoute] string LineCode)
        {
            try
            {
                var label = Labels.TryParseNewWBFormat(ezLabel);

                if (label == null)
                {
                    return BadRequest(new ApiResponse("Datos inválidos."));
                }

                var unitId = label.UnitID;

                if (!await TraceInEZ2000Motors(label.UnitID).ConfigureAwait(false))
                {
                    throw new TraceException("La etiqueta no ha sido trazada en la estación de Join Motors.");
                }
                else if (await TraceInProcessHistory(label.UnitID).ConfigureAwait(false))
                {
                    throw new TraceException("La etiqueta ya ha sido trazada en esta estación.");
                }
                else
                {
                    await RecordProcess2(label.UnitID, "0", LineCode).ConfigureAwait(false);
                    return new OkObjectResult(new ApiResponse($"Unidad {label.UnitID} registrada correctamente."));
                }
            }
            catch (TraceException ex)
            {
                return StatusCode(400, new ApiResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.Message));
            }
        }
        public class TraceException : Exception
        {
            public TraceException(string message) : base(message)
            { }
        }

        private async Task RecordProcess2(long unitID, string processNo, string lineCode)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            await con.ExecuteAsync(
                "INSERT INTO dbo.ProcessHistory (UnitID, ProcessID, LineCode) VALUES(@unitID, @processNo, @lineCode);",
                new { unitID, processNo, lineCode });
        }

        private async Task<bool> TraceInEZ2000Motors(long unitID)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            return con.ExecuteScalar<int>(
                "SELECT COUNT(DISTINCT(UnitID)) AS [EZ2000Motors] FROM EZ2000Motors WHERE UnitID = @unitID",
                new { unitID }) > 0;
        }
        private async Task<bool> TraceInProcessHistory(long unitID)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            return con.ExecuteScalar<int>(
                "SELECT COUNT(UnitID) AS UnitID  FROM [gtt].[dbo].[ProcessHistory] where UnitID = @unitID AND ProcessID = 0",
                new { unitID }) > 0;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        [Route("/api/ProductionID/{ProductionID}/Line/{LineCode}")]
        public async Task<IActionResult> Execute5([FromRoute] string ProductionID, [FromRoute] string LineCode)
        {
            try
            {
                if (string.IsNullOrEmpty(ProductionID))
                {
                    throw new TraceException("Datos inválidos.");
                }

                var label = Labels.ParseMotorData(ProductionID);
#pragma warning disable CS8602
                if (await TraceInMotorsDataFrameless(label.SerialNumber).ConfigureAwait(false))
                {
                    throw new TraceException($"La Unidad {label.SerialNumber} ya se encuentra registrada.");
                }

                await RecordProcess3(label.SerialNumber, label.Volt, label.RPM, label.DateTime, LineCode).ConfigureAwait(false);
                return Ok(new ApiResponse($"Unidad {label.SerialNumber} registrada correctamente."));
            }
            catch (TraceException ex)
            {
                return StatusCode(400, new ApiResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.Message));
            }
        }

        private async Task RecordProcess3(string SerialNumber, string Volt, string RPM, DateTime DateTimeMotor, string LineCode)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            await con.ExecuteAsync(
                "INSERT INTO dbo.MotorsData([SerialNumber],[Volt],[RPM],[DateTimeMotor],[Line])VALUES(@SerialNumber,@Volt,@RPM,@DateTimeMotor,@LineCode);",
                new { SerialNumber, Volt, RPM, DateTimeMotor, LineCode });
        }

        private async Task<bool> TraceInMotorsDataFrameless(string ProductionID)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            return con.ExecuteScalar<int>(
                "SELECT COUNT(DISTINCT(SerialNumber)) FROM MotorsData WHERE SerialNumber = @ProductionID",
                new { ProductionID }) > 0;
        }
        //------------------------------------------------
        #endregion

        public record ApiResponse(string Message);

        private static async Task<SqlConnection> GetOpenConnection(string connectionString)
        {
            var con = new SqlConnection(connectionString);
            await con.OpenAsync();
            return con;
        }

        private async Task<SqlConnection> GetGttConnection() =>
            await GetOpenConnection("Data Source=MXSRVAPPS\\SQLEXPRESS, 1433;Initial Catalog=gtt;UID=svc_trace_v2;PWD=svc_trace_v2;TrustServerCertificate=True;").ConfigureAwait(false);

        private async Task<SqlConnection> GetAppsConnection() =>
            await GetOpenConnection("Data Source=MXSRVTRACA;Initial Catalog=APPS;Persist Security Info=True;User ID=sa;Password=cegid.2008;TrustServerCertificate=True;MultipleActiveResultSets=TRUE;").ConfigureAwait(false);

        private async Task<string> GetUnitsCurrentProcess(long unitID, string lineCode)
        {
            using var con = await GetAppsConnection().ConfigureAwait(false);
            return await con.ExecuteScalarAsync<string>(
                "SELECT curr_process_no FROM pro_tms WHERE id=@unitID AND line = @lineCode;",
                new { unitID = unitID.ToString(), lineCode });
        }

        private async Task UpdateUnitsCurrentProcess(long unitID, string processNo)
        {
            using var con = await GetAppsConnection().ConfigureAwait(false);
            await con.ExecuteAsync(
                "UPDATE pro_tms SET curr_process_no=@processNo WHERE id=@unitID;",
                new { unitID = unitID.ToString(), processNo });
        }

        private async Task RecordProcess(long unitID, string processNo, string lineCode)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            await con.ExecuteAsync(
                "INSERT INTO dbo.ProcessHistory (UnitID, ProcessID, LineCode) VALUES(@unitID, @processNo, @lineCode);",
                new { unitID, processNo, lineCode });
        }

        private async Task<bool> RouteTransitionIsValid(string lineCode, string currProcessNo, string nextProcessNo)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            return con.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM LineRouting WHERE LineCode=@lineCode AND ProcessNo=@nextProcessNo AND PrevProcessNo=@currProcessNo;",
                new { lineCode, currProcessNo, nextProcessNo }) > 0;
        }
    }
}
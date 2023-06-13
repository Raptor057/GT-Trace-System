using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
//using System.Globalization;
using System.Text.RegularExpressions;

namespace GT.Trace.JoinMotors.HttpServices.EndPoints.Units.Lines.JoinMotorsEZ2000
{
    public record Ez2000Motor(string? Website, string? Voltage, string? RPM, string Date, string? Time, string? ProductionID);
    public record Label(long? UnitID);

    public static class Labels
    {
        public const string InformationSeparatorThree = "\u001d";
        public const string EndOfTransmission = "\u0004";
        private static string WalkBehindLabelFormatRegExPattern => @"\[\)>06SWB(?<transmissionID>\d+)P(?<clientPartNo>.+)Z.+1T(?<partNo>.+)2T(?<partRev>.+)3T(?<julianDay>\d+)$";
        private static string MotorsLabelFormatRegExPattern => @"^(?<website>.+)\s+(?<voltage>[0-9\.]+[A-Z])(?:\s+)?(?<rpm>[0-9]+)\s+(?<date>\d{4}-\d{1,2}-\d{1,2})\s+(?<time>\d{1,2}:\d{2})\s+(?<id>[0-9]+)$";
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
        public static Ez2000Motor? TryParseMotorsFormat(string value)
        {
            var match = Regex.Match(
            ClearInputFromSpecialCharacters(value),
            MotorsLabelFormatRegExPattern,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                return new Ez2000Motor(
                match.Groups["website"].Value,
                match.Groups["voltage"].Value,
                match.Groups["rpm"].Value,
                match.Groups["date"].Value,
                match.Groups["time"].Value,
                match.Groups["id"].Value);
            }
            else
            {
                return null;
            }
        }
    }

    [ApiController]
    public class JoinMotorsEZ2000EndPoint : ControllerBase
    {
        [HttpPost]
        [Route("/api/JoinMotorsEZ2000/{ezLabel}/{motor1}/{motor2}")]
        public async Task<IActionResult> Execute([FromRoute] string ezLabel, [FromRoute] string motor1, [FromRoute] string motor2)
        {
            try
            {
                var ez2000Motor1 = Labels.TryParseMotorsFormat(motor1);
                var ez2000Motor2 = Labels.TryParseMotorsFormat(motor2);
                var label = Labels.TryParseNewWBFormat(ezLabel);

                if (ez2000Motor1 == null || ez2000Motor2 == null || ezLabel == null)
                {
                    return BadRequest(new ApiResponse("Datos inválidos."));
                }

                #pragma warning disable CS8604
                await JoinMotors(label.UnitID ?? 0, ez2000Motor1.Website, ez2000Motor1.Voltage, ez2000Motor1.RPM, Convert.ToString(ez2000Motor1.Date), ez2000Motor1.Time, ez2000Motor1.ProductionID,
                    ez2000Motor2.Voltage, ez2000Motor2.RPM, Convert.ToString(ez2000Motor2.Date), ez2000Motor2.Time, ez2000Motor2.ProductionID).ConfigureAwait(false);
                #pragma warning restore CS8604

                return Ok(new ApiResponse($"Unidad {label.UnitID} casada con Motores {ez2000Motor1.ProductionID} y {ez2000Motor2.ProductionID} correctamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse($"Ocurrió un error al unir los motores: {ex.Message}"));
            }
         }
        public record ApiResponse(string Message);
        private static async Task<SqlConnection> GetOpenConnection(string connectionString)
        {
            var con = new SqlConnection(connectionString);
            await con.OpenAsync();
            return con;
        }
        private static async Task<SqlConnection> GetGttConnection() =>
        await GetOpenConnection("Data Source=MXSRVAPPS\\SQLEXPRESS, 1433;Initial Catalog=gtt;UID=svc_trace_v2;PWD=svc_trace_v2;TrustServerCertificate=True;").ConfigureAwait(false);

        private static async Task JoinMotors(long unitID, string web, string No_Load_Current, string No_Load_Speed, string date, string time, string Motor_number, string No_Load_Current2, string No_Load_Speed2, string date2, string time2, string Motor_number2)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            await con.ExecuteAsync(
                "INSERT INTO [dbo].[EZ2000Motors]([UnitID],[Website],[No_Load_Current],[No_Load_Speed],[Date],[Time],[Motor_number])VALUES" +
                "(@unitID,@web,@No_Load_Current,@No_Load_Speed,@date,@time,@Motor_number)," +
                "(@unitID,@web,@No_Load_Current2,@No_Load_Speed2,@date2,@time2,@Motor_number2)",
                new { unitID, web, No_Load_Current, No_Load_Speed, date, time, Motor_number, No_Load_Current2, No_Load_Speed2, date2, time2, Motor_number2 });
        }
    }
}
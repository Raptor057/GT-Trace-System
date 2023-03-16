using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GT.Trace.ProcessHistory.HttpServices.EndPoints.Units.Lines.Processes
{
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
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponse($"Unidad #{unitID} no encontrada en la línea {lineCode}."));
            }

            if (!await RouteTransitionIsValid(lineCode, currentUnitsProcess, processNo).ConfigureAwait(false))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse($"Unidad #{unitID} se encuentra en el proceso \"{currentUnitsProcess}\" el cual no es aceptable en el proceso \"{processNo}\" o el proceso \"{processNo}\" no se encuentra definido para la línea \"{lineCode}\"."));
            }

            await RecordProcess(unitID, processNo, lineCode).ConfigureAwait(false);

            await UpdateUnitsCurrentProcess(unitID, processNo).ConfigureAwait(false);

            return Ok(new ApiResponse("OK"));
        }

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
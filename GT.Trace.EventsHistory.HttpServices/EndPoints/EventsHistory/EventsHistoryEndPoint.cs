using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GT.Trace.EventsHistory.HttpServices.EndPoints.EventsHistory
{
    [ApiController]
    public class EventsHistoryEndPoint : Controller
    {
        private readonly ILogger<EventsHistoryEndPoint> _logger;

        public EventsHistoryEndPoint(ILogger<EventsHistoryEndPoint> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/message/{clientmessage}/lines/{lineCode}")]
        public async Task<IActionResult> Execute([FromRoute] string clientmessage, [FromRoute] string linecode)
        {
            await EventsHistory(clientmessage, linecode).ConfigureAwait(false);
            return Ok(new ApiResponse("OK"));
            //return Ok(new ApiResponse($"Event: {clientmessage} In: {linecode} At: {DateTime.Now}"));
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

        private async Task EventsHistory(string clientmessage, string lineCode)
        {
            using var con = await GetGttConnection().ConfigureAwait(false);
            await con.ExecuteAsync(
                "INSERT INTO dbo.EventsHistory (ClientMessage, LineCode) VALUES(@clientmessage, @lineCode);",
                new { ClientMessage = clientmessage, LineCode = lineCode });
        }


    }
}

using Dapper;
using GT.Trace.Changeover.App.UseCases.ApplyChangeover;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Transactions;

namespace GT.Trace.Changeover.UI.HttpApi.EndPoints.Lines.ApplyChangeover
{
    [ApiController]
    public class ApplyChangeoverEndPoint : ControllerBase
    {
        private readonly ILogger<ApplyChangeoverEndPoint> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<ApplyChangeoverEndPoint> _model;

        public ApplyChangeoverEndPoint(ILogger<ApplyChangeoverEndPoint> logger, IMediator mediator, GenericViewModel<ApplyChangeoverEndPoint> model)
        {
            _logger = logger;
            _mediator = mediator;
            _model = model;
        }

        /// <summary>
        /// get line
        /// get work order
        /// check for changeover
        /// if true
        ///      get component-point of use tuples not found in the incoming model
        ///      print return labels for etis with no expiration time matching the tuples
        ///      expire etis with no expiration time matching the tuples
        /// </summary>
        /// <param name="lineCode">Two-character line code.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/lines/{lineCode}/codew")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode)
        {
            var request = new ApplyChangeoverRequest(lineCode);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _model.IsSuccess ? Ok(_model) : StatusCode(500, _model);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException;
                return Ok(new { IsSuccess = false, Message = innerEx.Message });
            }
        }

        //        [HttpPut]
        //        [Route("api/lines/{lineCode}/codew")]
        //        public async Task<IActionResult> Execute([FromRoute] string lineCode)
        //        {
        //            lineCode = lineCode.ToUpper();
        //            try
        //            {
        //                dynamic pro_prod_units, pro_production;
        //                using (var con = new SqlConnection("Data Source=MXSRVTRACA;Initial Catalog=APPS;Persist Security Info=True;User ID=sa;Password=cegid.2008;TrustServerCertificate=True;MultipleActiveResultSets=TRUE;"))
        //                {
        //                    await con.OpenAsync().ConfigureAwait(false);
        //                    pro_prod_units = await con.QuerySingleOrDefaultAsync("SELECT * FROM dbo.pro_prod_units WHERE letter=@lineCode;", new { lineCode }).ConfigureAwait(false);
        //                    if (pro_prod_units == null)
        //                    {
        //                        return StatusCode((int)HttpStatusCode.NotFound, new { IsSuccess = false, Errors = new string[] { $"No se encontró la línea \"{lineCode}\"." } });
        //                    }
        //                    var lineID = pro_prod_units.id;
        //                    pro_production = await con.QuerySingleOrDefaultAsync("SELECT TOP 1 * FROM dbo.pro_production WHERE id_line=@lineID AND is_running=1 AND is_stoped=0 AND is_finished=0 ORDER BY last_update_time DESC;", new { lineID = (int)lineID }).ConfigureAwait(false);
        //                    if (pro_production == null)
        //                    {
        //                        return StatusCode((int)HttpStatusCode.NotFound, new { IsSuccess = false, Errors = new string[] { $"No se encontró órden de fabricación asociada a la línea #{lineID}." } });
        //                    }
        //                }

        //                var changeoverIsRequired = !(pro_prod_units.codew == pro_production.codew && pro_prod_units.modelo == pro_production.part_number.Trim());
        //                if (!changeoverIsRequired)
        //                {
        //                    return StatusCode((int)HttpStatusCode.Conflict, new { IsSuccess = false, Errors = new string[] { $"La línea \"{lineCode}\" no requiere un cambio de modelo." } });
        //                }

        //                IEnumerable<dynamic> outgoingComponents;
        //                using (var con = new SqlConnection("Data Source=MXSRVTRACA;Initial Catalog=TRAZAB;Persist Security Info=True;User ID=sa;Password=cegid.2008;TrustServerCertificate=True;MultipleActiveResultSets=TRUE;"))
        //                {
        //                    await con.OpenAsync().ConfigureAwait(false);
        //                    await con.ExecuteAsync("update apps.dbo.pro_prod_units set modelo=RTRIM(@part_number), active_revision=RTRIM(@rev), codew=@codew where id=@id_line;", (object)pro_production).ConfigureAwait(false);
        //                    var args = new { ogPartNo = pro_prod_units.modelo, icPartNo = pro_production.part_number, lineCode };
        //                    outgoingComponents = await con.QueryAsync(@"SELECT CompNo, CompRev2, PointOfUse FROM cegid.ufn_bom(@ogPartNo, @lineCode)
        //EXCEPT
        //SELECT CompNo, CompRev2, PointOfUse FROM cegid.ufn_bom(@icPartNo, @lineCode)", args).ConfigureAwait(false);
        //                }

        //                List<dynamic> outgoingEtis = new();
        //                using (var con = new SqlConnection("Data Source=MXSRVAPPS\\SQLEXPRESS, 1433;Initial Catalog=gtt;UID=svc_trace_v2;PWD=svc_trace_v2;TrustServerCertificate=True;"))
        //                {
        //                    await con.OpenAsync().ConfigureAwait(false);
        //                    using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //                    {
        //                        foreach (var item in outgoingComponents)
        //                        {
        //                            outgoingEtis.AddRange(
        //                                await con.QueryAsync("SELECT * FROM dbo.PointOfUseEtis WITH(NOLOCK) WHERE UtcExpirationTime IS NULL AND ComponentNo = @CompNo AND PointOfUseCode = @PointOfUse;", (object)item).ConfigureAwait(false)
        //                            );
        //                            await con.ExecuteAsync("UPDATE dbo.PointOfUseEtis SET UtcExpirationTime=GETUTCDATE(), Comments = 'CHANGEOVER' WHERE UtcExpirationTime IS NULL AND ComponentNo = @CompNo AND PointOfUseCode = @PointOfUse;", (object)item).ConfigureAwait(false);
        //                        }
        //                        tx.Complete();
        //                    }
        //                }

        //                var errors = new List<string>();
        //                using (var client = new HttpClient())
        //                {
        //                    var uri = new Uri($"http://mxsrvapps.gt.local/gtt/services/etimovements/api/lines/{lineCode}/etis");
        //                    client.BaseAddress = uri;
        //                    foreach (var eti in outgoingEtis)
        //                    {
        //                        var data = JsonConvert.SerializeObject(new { EtiInput = $"{eti.EtiNo}", IsReturn = true });
        //                        var content = new StringContent(data, Encoding.UTF8, "application/json");
        //                        using var response = await client.PutAsync(uri, content);
        //                        if (!response.IsSuccessStatusCode)
        //                        {
        //                            var responseContent = await response.Content.ReadAsStringAsync();
        //                            var jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(responseContent);
        //                            errors.Add(jsonResponse!.Message);
        //                        }
        //                    }
        //                }

        //                return Ok(new { IsSuccess = true, Errors = errors.ToArray(), Data = DateTime.UtcNow });
        //            }
        //            catch (Exception ex)
        //            {
        //                var innerEx = ex;
        //                while (innerEx.InnerException != null) innerEx = innerEx.InnerException;
        //                return Ok(new { IsSuccess = false, Message = innerEx.Message });
        //            }
        //        }

        //        private sealed record JsonResponse(string Message);
    }
}
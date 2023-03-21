using GT.Trace.App.UseCases.MaterialLoading.FetchLines;
using GT.Trace.App.UseCases.MaterialLoading.FetchLineWorkOrders;
using GT.Trace.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints
{
    [ApiController]
    [Route("[controller]")]
    public class LinesController : ControllerBase
    {
        private readonly ILogger<LinesController> _logger;

        private readonly IMediator _mediator;

        public LinesController(ILogger<LinesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/api/lines")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new FetchLinesRequest()).ConfigureAwait(false);
                if (response is IFailure failure)
                {
                    return StatusCode(500, new { IsSuccess = false, failure.Message, UtcTimeStamp = DateTime.UtcNow });
                }
                return Ok(new { IsSuccess = true, UtcTimeStamp = DateTime.UtcNow, Data = (response as ISuccess<FetchLinesResponse>)?.Data.Lines });
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, new { IsSuccess = false, innerEx.Message, UtcTimeStamp = DateTime.UtcNow });
            }
        }

        [HttpGet]
        [Route("/api/lines/{lineID}/workorders")]
        public async Task<IActionResult> Get([FromRoute] int lineID)
        {
            try
            {
                var response = await _mediator.Send(new FetchLineWorkOrdersRequest(lineID)).ConfigureAwait(false);
                if (response is IFailure failure)
                {
                    return StatusCode(500, new { IsSuccess = false, failure.Message, UtcTimeStamp = DateTime.UtcNow });
                }
                return Ok(new { IsSuccess = true, UtcTimeStamp = DateTime.UtcNow, Data = (response as ISuccess<FetchLineWorkOrdersResponse>)?.Data.WorkOrders });
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, new { IsSuccess = false, innerEx.Message, UtcTimeStamp = DateTime.UtcNow });
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.CegidRadioWebApi.EndPoints.ReportFabricationControl
{
    [ApiController]
    [Route("[controller]")]
    public sealed class CreateFabricationControlEntryController : ControllerBase
    {
        private readonly CreateFabricationControlHandler _handler;

        public CreateFabricationControlEntryController(CreateFabricationControlHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [Route("/api/fabricationcontrol")]
        public async Task<IActionResult> ExecuteAsync([FromBody] CreateFabricationControlEntryRequest request)
        {
            if (!_handler.CanExecute(request, out var errors))
            {
                return BadRequest(HttpApiResponse.Fail(errors.Select(e => $"- {e}").Aggregate((a, b) => $"{a}\n{b}")));
            }
            try
            {
                await _handler.ExecuteAsync(request).ConfigureAwait(false);
                return Ok(HttpApiResponse.OK());
            }
            catch (Exception ex)
            {
                return StatusCode(500, HttpApiResponse.Fail(ex.Message));
            }
        }
    }
}
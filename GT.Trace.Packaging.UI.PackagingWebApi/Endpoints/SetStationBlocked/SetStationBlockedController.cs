using GT.Trace.Packaging.App.UseCases.SetStationBlocked;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.SetStationBlocked
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetStationBlockedController : ControllerBase
    {
        private readonly ILogger<SetStationBlockedController> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<SetStationBlockedController> _viewModel;

        public SetStationBlockedController(ILogger<SetStationBlockedController> logger, IMediator mediator, GenericViewModel<SetStationBlockedController> viewModel)
        {

            _logger=logger;
            _mediator=mediator;
            _viewModel=viewModel;
        }

        [HttpPut]
        [Route ("/api/StationBlocked/{is_blocked}/{lineName}")]
        public async Task<IActionResult> Get([FromRoute] string is_blocked, [FromRoute] string lineName)
        {
            var request = new SetStationBlockedRequest(is_blocked, lineName);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                _logger.LogError(Convert.ToString(innerEx)/*, "Ha ocurrido un error al procesar la solicitud"*/); //Nueva linea agregada
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }
    }
}

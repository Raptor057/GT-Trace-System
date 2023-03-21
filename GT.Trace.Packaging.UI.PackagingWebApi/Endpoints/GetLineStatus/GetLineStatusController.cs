using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.LoadPackState;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.GetLineStatus
{
    [ApiController]
    [Route("[controller]")]
    public class GetLineStatusController : ControllerBase
    {
        private readonly ILogger<GetLineStatusController> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<GetLineStatusController> _viewModel;

        public GetLineStatusController(ILogger<GetLineStatusController> logger, IMediator mediator, GenericViewModel<GetLineStatusController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/lines/{hostname}")]
        public async Task<IActionResult> Get([FromRoute] string hostname, [FromQuery] string? lineCode, [FromQuery] int? containerSize, [FromQuery] int? palletSize, [FromQuery] string? poNumber)
        {
            var request = new LoadPackStateRequest(hostname, palletSize, containerSize, lineCode, poNumber);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }
    }
}
using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.GetHourlyProduction;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.GetHourlyProduction
{
    [ApiController]
    [Route("[controller]")]
    public class GetHourlyProductionController : ControllerBase
    {
        private readonly ILogger<GetHourlyProductionController> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<GetHourlyProductionController> _viewModel;

        public GetHourlyProductionController(ILogger<GetHourlyProductionController> logger, IMediator mediator, GenericViewModel<GetHourlyProductionController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/lines/{lineCode}/hourlyproduction")]
        public async Task<IActionResult> Get([FromRoute] string lineCode)
        {
            var request = new GetHourlyProductionRequest(lineCode);
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
using GT.Trace.App.UseCases.Lines.GetHourlyProduction;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetHourlyProduction
{
    [ApiController]
    public class GetHourlyProductionController : ControllerBase
    {
        private readonly ILogger<GetHourlyProductionController> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<GetHourlyProductionController> _viewModel;

        public GetHourlyProductionController(ILogger<GetHourlyProductionController> logger, IMediator mediator, ResultViewModel<GetHourlyProductionController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/lines/{lineCode}/production")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode)
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
using GT.Trace.App.UseCases.Lines.GetCurrentHourProduction;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetCurrentHourProduction
{
    [ApiController]
    public class GetCurrentHourProductionEndPoint : ControllerBase
    {
        private readonly ILogger<GetCurrentHourProductionEndPoint> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<GetCurrentHourProductionEndPoint> _viewModel;

        public GetCurrentHourProductionEndPoint(ILogger<GetCurrentHourProductionEndPoint> logger, IMediator mediator, ResultViewModel<GetCurrentHourProductionEndPoint> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/lines/{lineCode}/production/hours/current")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode)
        {
            var request = new GetCurrentHourProductionRequest(lineCode);
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
using GT.Trace.App.UseCases.Lines.GetPointsOfUse;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetPointsOfUse
{
    [ApiController]
    public class GetPointsOfUseController : ControllerBase
    {
        private readonly ILogger<GetPointsOfUseController> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<GetPointsOfUseController> _viewModel;

        public GetPointsOfUseController(ILogger<GetPointsOfUseController> logger, IMediator mediator, ResultViewModel<GetPointsOfUseController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/lines/{lineCode}/pointsofuse")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode)
        {
            var request = new GetPointsOfUseRequest(lineCode);
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
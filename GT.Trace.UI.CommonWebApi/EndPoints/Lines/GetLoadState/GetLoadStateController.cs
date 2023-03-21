using GT.Trace.App.UseCases.Lines.GetLoadState;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetLoadState
{
    [ApiController]
    [Route("[controller]")]
    public partial class GetLoadStateController : ControllerBase
    {
        private readonly ILogger<GetLoadStateController> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<GetLoadStateController> _viewModel;

        public GetLoadStateController(GenericViewModel<GetLoadStateController> viewModel, ILogger<GetLoadStateController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/lines/{lineCode}/etis/{partNo}")]
        public async Task<IActionResult> ExecuteAsync([FromRoute] string lineCode, [FromRoute] string partNo)
        {
            try
            {
                var result = await _mediator.Send(new GetLoadStateRequest(lineCode, partNo)).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innermostEx = ex;
                while (innermostEx.InnerException != null) innermostEx = innermostEx.InnerException!;
                return StatusCode(500, new { innermostEx.Message, IsSuccess = false, UtcTimeStamp = DateTime.UtcNow });
            }
        }
    }
}
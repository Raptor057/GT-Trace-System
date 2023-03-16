using GT.Trace.App.UseCases.Lines.GetLine;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.Lines.GetLine
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetLineController : ControllerBase
    {
        private readonly ILogger<GetLineController> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<GetLineController> _viewModel;

        public GetLineController(ResultViewModel<GetLineController> viewModel, ILogger<GetLineController> logger, IMediator mediator)
        {
            _viewModel = viewModel;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/api/lines/{lineCode}")]
        public async Task<IActionResult> ExecuteAsync([FromRoute] string lineCode)
        {
            try
            {
                if (!GetLineRequest.CanCreate(lineCode, out var errors))
                {
                    return StatusCode(400, _viewModel.Fail(errors.Select(e => $"- {e}").Aggregate((a, b) => $"{a}\n{b}")));
                }
                await _mediator.Send(GetLineRequest.Create(lineCode)).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innermostEx = ex;
                while (innermostEx.InnerException != null) innermostEx = innermostEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innermostEx.Message));
            }
        }
    }
}
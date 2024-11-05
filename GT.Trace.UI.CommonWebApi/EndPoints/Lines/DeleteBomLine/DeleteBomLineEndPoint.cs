using GT.Trace.App.UseCases.Lines.DeleteBomLine;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.DeleteBomLine
{
    [ApiController]
    public class DeleteBomLineEndPoint : ControllerBase
    {
        private readonly ILogger<DeleteBomLineEndPoint> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<DeleteBomLineEndPoint> _viewModel;

        public DeleteBomLineEndPoint(ILogger<DeleteBomLineEndPoint> logger, IMediator mediator, GenericViewModel<DeleteBomLineEndPoint> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpPut]
        [Route("api/lines/deletegama/partno/{ogpartNo}/{icpartNo}/lineCode/{oglineCode}/{iclineCode}")]
        public async Task<IActionResult> Execute([FromRoute] string ogpartNo, [FromRoute] string icpartNo, [FromRoute] string oglineCode, [FromRoute] string iclineCode)
        {
            var request = new DeleteBomLineRequest(ogpartNo, icpartNo, oglineCode, iclineCode);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException;
                return Ok(new { IsSuccess = false, Message = innerEx.Message });
            }
        }
    }
}

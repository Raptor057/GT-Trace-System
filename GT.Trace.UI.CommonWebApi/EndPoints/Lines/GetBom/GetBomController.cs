using GT.Trace.App.UseCases.Lines.GetBom;
using GT.Trace.App.UseCases.Lines.GetWorkOrder;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetBom
{
    [ApiController]
    public class GetBomController : ControllerBase
    {
        private readonly ILogger<GetBomController> _logger;

        private readonly ResultViewModel<GetBomController> _viewModel;

        private readonly IMediator _mediator;

        public GetBomController(ILogger<GetBomController> logger, ResultViewModel<GetBomController> viewModel, IMediator mediator)
        {
            _logger = logger;
            _viewModel = viewModel;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/lines/{lineCode}/bom/{partNo}")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode, [FromRoute] string partNo)
        {
            var request = new GetBomRequest(partNo, lineCode);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var exception = ex;
                while (exception.InnerException != null) exception = ex.InnerException!;
                return StatusCode(500, _viewModel.Fail(exception.Message));
            }
        }
    }
}